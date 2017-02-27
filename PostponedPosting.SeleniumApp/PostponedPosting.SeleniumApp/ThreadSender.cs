using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using PostponedPosting.Domain.Entities.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PostponedPosting.SeleniumApp
{
    class ThreadSender
    {
        public event EventHandler ThreadDone;
        public Post Post { get; set; }
        public IWebDriver Driver { get; set; }

        public void Run()
        {
            // Do a task
            var message = Post.Content;
            var links = Post.GroupsOfLinks.SelectMany(l => l.Links.Select(u => u.Url)).ToArray();
           
            for (int i = 0; i < links.Count(); i++)
            {
                Driver.Navigate().GoToUrl(links[i]);
                if (!links[i].Contains("groups"))
                {
                    //Post on facebook user page
                    try
                    {
                        Thread.Sleep(2000);
                        var element = Driver.FindElement(By.XPath("//textarea[contains(@placeholder, 'О чем вы думаете')]"));
                        element.SendKeys(message);
                        Thread.Sleep(5000);
                        ((IJavaScriptExecutor)Driver).ExecuteScript("document.getElementById('pagelet_composer').querySelector('[type = submit]').click()");
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    //Post on group page
                    try
                    {                        
                        Thread.Sleep(2000);
                        var element = Driver.FindElement(By.XPath("//textarea[contains(@placeholder, 'Напишите')]"));
                        element.SendKeys(message);            
                        Thread.Sleep(5000);
                        element = Driver.FindElement(By.XPath("//button//em[@data-intl-translation='Опубликовать']/ancestor::button"));
                        element.Click();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            if (ThreadDone != null)
                ThreadDone(this, EventArgs.Empty);
        }
    }
}
