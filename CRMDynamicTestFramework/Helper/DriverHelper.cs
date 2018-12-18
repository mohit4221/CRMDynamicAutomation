using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRMDynamicTestFramework
{
    public static class DriverHelper
    {
        public static IWebElement FindElementByClassName(this IWebDriver driver, string className, bool isClickable = false)
        {
            if (isClickable)
                WaitForClickable(driver, By.ClassName(className), true);
            else
                WaitForIsVisible(driver, By.ClassName(className));

            var element = driver.FindElement(By.ClassName(className));
            Assert.IsNotNull(element, string.Format("Element not found by className: {0}.", className));

            return element;
        }

        public static void WaitForClickable(this IWebDriver driver, By searchBy, bool byClassName = false)
        {
            //wait for a UI Element to be found
            WebDriverWait wait = new WebDriverWait(driver, Constant.WAIT_FOR_EXIST_SEC);
            wait.Until(ExpectedConditions.ElementToBeClickable(searchBy));

            //Wait for clickable not working for classname, added sleep for that. 
            if (byClassName)
                Thread.Sleep(5000);
        }

        public static void WaitForIsVisible(this IWebDriver driver, By searchBy)
        {
            //wait for a UI Element to be found
            WebDriverWait wait = new WebDriverWait(driver, Constant.WAIT_FOR_EXIST_SEC);
            wait.Until(ExpectedConditions.ElementIsVisible(searchBy));
        }
    }
}
