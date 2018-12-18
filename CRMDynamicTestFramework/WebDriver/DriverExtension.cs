using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace CRMDynamicTestFramework
{
    public abstract class DriverExtension
    {
        public abstract IWebDriver Driver
        {
            get;
        }

        public string CurrentWindowHandle
        {
            get { return this.Driver.CurrentWindowHandle; }
        }

        public AppType CurrentRunningEnvironment { get; set; }

        public void Quit()
        {
            this.Driver.Quit();
        }

        public string ExecuteScript(string script)
        {
            string scriptResult = null;
            var result = ((IJavaScriptExecutor)this.Driver).ExecuteScript(script);
            scriptResult = result?.ToString();
            return scriptResult;
        }

        public IWebElement FindVisibleElementInElements(By searchBy)
        {
            // find visible element
            var allElements = this.SearchElements(searchBy);
            foreach (IWebElement element in allElements)
            {
                if (element.Displayed)
                {
                    return element;
                }
            }

            Assert.Fail("No Visible Element found by : {0}.", searchBy);
            return null;
        }

        public IWebElement Search( By searchBy, int maxAttempts = 5)
        {
            return this.SearchWebElement(null, searchBy, maxAttempts);
        }

        public IWebElement Search(IWebElement parent, By searchBy, int maxAttempts = 5)
        {
            return this.SearchWebElement(parent, searchBy, maxAttempts);
        }

        public ReadOnlyCollection<IWebElement> SearchElements(By searchBy, int maxAttempts = 5)
        {
            return this.SearchElements(null, searchBy, maxAttempts);
        }

        public ReadOnlyCollection<IWebElement> SearchElements(IWebElement parent, By searchBy, int maxAttempts = 5)
        {
            return this.SearchWebElements(parent, searchBy, maxAttempts);
        }
        
        private IWebElement SearchWebElement(IWebElement parent, By searchBy, int maxAttempts = 5)
        {

            IWebElement result = null;
            Func<bool> f = () =>
            {
                try
                {
                    result = parent != null ? parent.FindElement(searchBy) : this.Driver.FindElement(searchBy);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            };

            f.ExecuteWithRetry(ApplicationConfig.SleepIntervalWaitTime.Milliseconds, maxAttempts);
            return result;
        }

        private ReadOnlyCollection<IWebElement> SearchWebElements(IWebElement parent, By searchBy, int maxAttempts = 5)
        {
            ReadOnlyCollection<IWebElement> result = null;
            Func<bool> f = () =>
            {
                try
                {
                    result = parent != null ? parent.FindElements(searchBy) : this.Driver.FindElements(searchBy);
                    return result.Count != 0;
                }
                catch (Exception e)
                {
                    return false;
                }
            };

            f.ExecuteWithRetry(ApplicationConfig.SleepIntervalWaitTime.Milliseconds, maxAttempts);
            return result;
        }
    }
}