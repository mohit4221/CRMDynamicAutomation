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

      
    }
}
