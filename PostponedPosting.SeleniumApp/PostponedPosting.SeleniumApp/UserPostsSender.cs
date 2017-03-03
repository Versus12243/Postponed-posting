using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.SeleniumApp.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PostponedPosting.SeleniumApp
{
    public class UserPostsSender
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public readonly string Id = null;
        object locker = new object();
        Stack<Post> Posts = null;
        RemoveDriver removeDriverDelegate = null;
        public readonly bool LoginSuccessful = false;

        private IWebDriver driver;

        public void SetupDriver()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.AddArgument("--window-position=-32000,-32000");

            driver = new ChromeDriver(service, options);
        }

        public UserPostsSender(string id, string login, string password, RemoveDriver removeDriver)
        {           
            Id = id;

            removeDriverDelegate = removeDriver;

            Posts = new Stack<Post>();

            SetupDriver();

            var baseURL = "http://facebook.com";

            #region auth
                driver.Navigate().GoToUrl(baseURL);
                driver.FindElement(By.XPath("//input[@id = 'email']")).Clear();
                //логин пользователя
                driver.FindElement(By.XPath("//input[@id = 'email']")).SendKeys(login);
                driver.FindElement(By.XPath("//input[@id = 'pass']")).Clear();
                //пароль пользователя
                driver.FindElement(By.XPath("//input[@id = 'pass']")).SendKeys(password);
                Thread.Sleep(2000);
                ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('loginbutton').click();");
                Thread.Sleep(2000);
            #endregion

            try
            {
                driver.FindElement(By.XPath("//button[@name = 'login']"));
                LoginSuccessful = false;
                logger.Error("Login failed in facebook provider for user with id " + Id);
                driver.Dispose();
            }
            catch
            {
                LoginSuccessful = true;
            }
        }

        public void AddPost(Post post)
        {
            lock (locker)
            {                
                Posts.Push(post);
            }
        }

        public void HandleThreadDone(object sender, EventArgs e)
        {
            var count = 0;

            lock(locker)
            {
                count = Posts.Count;
            }

            if (count > 0)
            {
                FacebookThreadSender senderThread = new FacebookThreadSender();
                senderThread.ThreadDone += HandleThreadDone;
                senderThread.Driver = driver;

                lock (locker)
                {
                    senderThread.Post = Posts.Pop();
                }
                Thread thread = new Thread(senderThread.Run);
                thread.Name = "UserPostsSender_" + this.Id;
                thread.Start();
            }
            else
            {
                driver.Dispose();
                removeDriverDelegate(this.Id);
            }        
        }        
    }
}
