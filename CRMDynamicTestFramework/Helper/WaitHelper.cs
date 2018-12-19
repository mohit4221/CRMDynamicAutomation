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
        public static void SmallWait(int n = 1)
        {
            Thread.Sleep(n * 100);
        }

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

        public static void WaitForClickable(this IWebDriver driver, By searchBy, TimeSpan timeOut)
        {
            //wait for a UI Element to be found
            WebDriverWait wait = new WebDriverWait(driver, timeOut);
            wait.Until(ExpectedConditions.ElementToBeClickable(searchBy));
        }

        public static void WaitForIsVisible(this IWebDriver driver, By searchBy, TimeSpan timeOut)
        {
            //wait for a UI Element to be found
            WebDriverWait wait = new WebDriverWait(driver, timeOut);
            wait.Until(ExpectedConditions.ElementIsVisible(searchBy));
        }

        public static void WaitForInvisible(this IWebDriver driver, By searchBy, TimeSpan timeOut)
        {
            //wait for a UI Element to be not found
            WebDriverWait wait = new WebDriverWait(driver, timeOut);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(searchBy));
        }

        public static void WaitForStaleness(this IWebDriver driver, IWebElement element, TimeSpan timeOut)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeOut);
            wait.Until(ExpectedConditions.StalenessOf(element));
        }

        public static void WaitForPresenceOfElement(this IWebDriver driver, By searchBy, TimeSpan timeOut)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeOut);
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(searchBy));
        }

        public static void WaitForControlToVisible(IWebElement control, By searchBy, int timeout)
        {
            bool res = WaitTrue(() => (control.FindElement(searchBy).Displayed == true), timeout);
            if (!res)
            {
                Assert.Fail("Control didn't display");
            }
        }

        public static void WaitForNewWindowAndSwitchToIt(this IWebDriver driver, TimeSpan timeOut)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeOut);
            if (wait.Until<bool>((d) => { return driver.WindowHandles.Count > 1; }))
                driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public static bool WaitForPageReady(this IWebDriver driver,  Func<IWebDriver, bool> condition, TimeSpan timeout)
        {
            var wait = new WebDriverWait(new SystemClock(), driver, timeout, ApplicationConfig.SleepIntervalWaitTime);
            var success = wait.Until(condition);
            return success;
        }

        public static void WaitForNewWindowAndSwitchToIt(this IWebDriver driver, IWebElement clickElement)
        {
            PopupWindowFinder waitForNewWindow = new PopupWindowFinder(driver);
            string newWindowHandler = waitForNewWindow.Click(clickElement);
            driver.SwitchTo().Window(newWindowHandler);
        }
    }
}
