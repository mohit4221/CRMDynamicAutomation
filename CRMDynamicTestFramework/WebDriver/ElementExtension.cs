using System;
using System.Threading;
using OpenQA.Selenium;

namespace CRMDynamicTestFramework
{
    public static class ElementExtension
    {
        public static void EnterText(this IWebElement element, string value)
        {
            element.SendKeys(value);
        }

        ///<summary>
        /// Extended method for click into button
        /// </summary>
        public static void Clicks(this IWebElement element)
        {
            element.Click();
        }

        public static string GetElementId(this IWebElement element, string elementCssClassName)
        {
            return element.FindElement(By.ClassName(elementCssClassName)).GetAttribute("id");
        }

        public static string GetElementText(this IWebElement element, string elementClassName)
        {
            return element.FindElement(By.ClassName(elementClassName)).Text;
        }

        public static bool ExecuteWithRetry(this Func<bool> f, int retryInterval = 500, int maxAttempts = 10)
        {
            var done = false;
            for (var attempts = 1; !done && attempts <= maxAttempts; attempts++)
            {
                done = f();

                if (!done)
                {
                    if (attempts < maxAttempts)
                    {
                        Thread.Sleep(retryInterval);
                    }
                }
            }

            return done;
        }

    }
}
