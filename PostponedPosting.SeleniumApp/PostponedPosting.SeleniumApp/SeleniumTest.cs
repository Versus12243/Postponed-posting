using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;
using System;

namespace PostponedPosting.SeleniumApp
{
    class SeleniumModule
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private string _currentUserId;

        public SeleniumModule(string userId)
        {
            _currentUserId = userId;
        }

        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "http://facebook.com";
            verificationErrors = new StringBuilder();                           
        }        
        
        public void The1Test()
        {           
            string message = "New post https://www.facebook.com/";
            #region auth
            driver.Navigate().GoToUrl(baseURL);
            driver.FindElement(By.XPath("//input[@id = 'email']")).Clear();
            //логин пользователя
            driver.FindElement(By.XPath("//input[@id = 'email']")).SendKeys("viktor16s@meta.ua");
            driver.FindElement(By.XPath("//input[@id = 'pass']")).Clear();
            //пароль пользователя
            driver.FindElement(By.XPath("//input[@id = 'pass']")).SendKeys("Koroche16");
            Thread.Sleep(1000);
            ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('loginbutton').click();");
            #endregion
            
            //дальше вводим пост и жмём кнопку 
            try
            {
                Thread.Sleep(2000);
                var element = driver.FindElement(By.XPath("//textarea[contains(@placeholder, 'О чем вы думаете')]"));
                element.SendKeys(message);
                ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementsByTagName('form')[1].getElementsByTagName('textarea')[0].click()");
                Thread.Sleep(5000);
                ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('pagelet_composer').querySelector('[type = submit]').click()");
            }
            catch (Exception ex)
            {
            }
            //try
            //{
            //    driver.Navigate().GoToUrl("https://www.facebook.com/BookShopIn?fref=ts");
            //    Thread.Sleep(1000);
            //    //$x(("//div[@aria-autocomplete='list']"))
            //    var element = driver.FindElement(By.XPath("//textarea[contains(@placeholder, 'Напишите')]"));
            //    element.SendKeys(message);  
            //}
            //catch (Exception ex)
            //{       
            //}
       }       
    }
}
