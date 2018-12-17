using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace CRMDynamicTestFramework
{
    public static class WaitHelper
    {
      
        private static bool WaitTrue(Func<bool> expr, int timeout)
        {
            DateTime startTime = DateTime.Now;

            while (!expr())
            {
                SmallWait();
                if ((DateTime.Now - startTime) > TimeSpan.FromMilliseconds(timeout))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Small 100ms sleep for waiters.
        /// </summary>
        public static void SmallWait(int n = 1)
        {
            Thread.Sleep(n * 100);
        }

        public static void WaitForClickable(this IWebDriver driver, By searchBy, bool byClassName = false)
        {
            //wait for a UI Element to be found
            WebDriverWait wait = new WebDriverWait(driver, Constant.WAIT_FOR_EXIST_SEC);
            wait.Until(ExpectedConditions.ElementToBeClickable(searchBy));

            //Wait for clickable not working for classname, added sleep for that. 
            //Bug 8087 : Selenium ExpectedConditions.ElementToBeClickable is not working for ClassName
            if (byClassName)
                Thread.Sleep(5000);
        }

        public static void WaitForIsVisible(this IWebDriver driver, By searchBy)
        {
            //wait for a UI Element to be found
            WebDriverWait wait = new WebDriverWait(driver, Constant.WAIT_FOR_EXIST_SEC);
            wait.Until(ExpectedConditions.ElementIsVisible(searchBy));
        }

        public static void WaitForNewWindowAndSwitchToIt(this IWebDriver driver)
        {
            //TODO: remove sleep when bug 8088: selenium driver get new driver.WindowHandles count but window take time to render fixed
            //PopupWindowFinder waitForNewWindow
            Thread.Sleep(10000);
            WebDriverWait wait = new WebDriverWait(driver, Constant.WAIT_FOR_EXIST_SEC);
            if (wait.Until<bool>((d) => { return driver.WindowHandles.Count > 1; }))
                driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public static void WaitForNewWindowAndSwitchToIt(this IWebDriver driver, IWebElement clickElement)
        {
            PopupWindowFinder waitForNewWindow = new PopupWindowFinder(driver);
            string newWindowHandler = waitForNewWindow.Click(clickElement);
            driver.SwitchTo().Window(newWindowHandler);
        }
    }
}
