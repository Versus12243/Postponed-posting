using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using PostponedPosting.Domain.Entities.PostModels;
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
        public readonly string Id = null;
        object locker = new object();
        Stack<Post> Posts = null;
        bool postSended = false;
        RemoveDriver removeDriverDelegate = null;

        private IWebDriver driver;

        public void SetupDriver()
        {
            driver = new FirefoxDriver();
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
                ThreadSender senderThread = new ThreadSender();
                senderThread.ThreadDone += HandleThreadDone;
                senderThread.Driver = driver;

                lock (locker)
                {
                    senderThread.Post = Posts.Pop();
                }
                Thread thread = new Thread(senderThread.Run);
                thread.Start();
            }
            else
            {
                driver.Dispose();                
            }        
        }        
    }
}
