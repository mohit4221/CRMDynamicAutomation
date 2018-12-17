using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Threading;

namespace CRMDynamicTestFramework
{
    public abstract class DriverExtension
    {
      
        protected string mainWindowHandler;


        public abstract IWebDriver Driver
        {
            get;
        }

        public DriverExtension()
        {
        }

        public string CurrentWindowHandle
        {
            get { return this.Driver.CurrentWindowHandle; }
        }
      
        public void skipFirstTimeLogin(AppType currentEnvironment)
        {
            if (currentEnvironment == AppType.POS)
            {
                IJavaScriptExecutor js = this as IJavaScriptExecutor;
                var data = js.ExecuteScript("return MagicGlass.UI.TestHooks.skipFirstTimeLogin()");
            }
            if (currentEnvironment == AppType.Web)
            {
                IWebElement skipButton = this.FindElementByClassName(Constant.SkipButton, true);
                this.TakeScreenShot("BeforeSkipButtonClick.png");
                skipButton.Clicks();
            }
        }

      

        /// <summary>
        /// Find element
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isClickable"></param>
        /// <returns></returns>       
        public IWebElement FindElementById(string id, bool isClickable = false)
        {
            // Wait for element to visible and clickable
            if (isClickable)
                WaitForClickable(By.Id(id));
            else
                WaitForIsVisible(By.Id(id));

            var element = this.Driver.FindElement(By.Id(id));
            Assert.IsNotNull(element, string.Format("Element not found by Id: {0}.", id));
            return element;
        }

        public IWebElement FindElementByXpath(string xpath, bool isClickable = false)
        {
            // Wait for element to visible and clickable
            if (isClickable)
                WaitForClickable(By.XPath(xpath));
            else
                WaitForIsVisible(By.XPath(xpath));

            var element = this.Driver.FindElement(By.XPath(xpath));
            Assert.IsNotNull(element, string.Format("Element not found by Id: {0}.", xpath));
            return element;
        }

        public IWebElement FindElementByClassName(string className, bool isClickable = false)
        {
            if (isClickable)
                WaitForClickable(By.ClassName(className), true);
            else
                WaitForIsVisible(By.ClassName(className));

            var element = this.Driver.FindElement(By.ClassName(className));
            Assert.IsNotNull(element, string.Format("Element not found by className: {0}.", className));

            return element;
        }

        public IWebElement FindElementByCssSelector(string className, bool isClickable = false)
        {
            if (isClickable)
                WaitForClickable(By.CssSelector(className), true);
            else
                WaitForIsVisible(By.CssSelector(className));

            var element = Driver.FindElement(By.CssSelector(className));
            return element;
        }
        public IWebElement FindVisibleElementByClassName(string className)
        {
            // find visible element
            var allElements = this.Driver.FindElements(By.ClassName(className));
            foreach (IWebElement element in allElements)
            {
                if (element.Displayed)
                {
                    return element;
                }
            }
            Assert.Fail("No Visible Element found by className: {0}.", className);
            return null;
        }

        public IWebElement FindVisibleElementByCssSelector(string CssSelector)
        {
            // find visible element
            var allElements = this.Driver.FindElements(By.CssSelector(CssSelector));
            foreach (IWebElement element in allElements)
            {
                if (element.Displayed)
                {
                    return element;
                }
            }
            Assert.Fail("No Visible Element found by className: {0}.", CssSelector);
            return null;
        }
        /// <summary>
        /// Wait for new login window and switch to new window
        /// </summary>
        public void WaitForNewWindowAndSwitchToIt()
        {
            //TODO: remove sleep when bug 8088: selenium Driver get new Driver.WindowHandles count but window take time to render fixed
            Thread.Sleep(10000);
            WebDriverWait wait = new WebDriverWait(Driver, Constant.WAIT_FOR_EXIST_SEC);
            if (wait.Until<bool>((d) => { return Driver.WindowHandles.Count > 1; }))
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
        }

        public void WaitForNewWindowAndSwitchToIt(IWebElement clickElement)
        {
            //TODO: remove sleep when bug 8088: selenium Driver get new Driver.WindowHandles count but window take time to render fixed
            PopupWindowFinder waitForNewWindow = new PopupWindowFinder(Driver);
            string newWindowHandler = waitForNewWindow.Click(clickElement);
            Driver.SwitchTo().Window(newWindowHandler);
        }

        /// <summary>
        /// Switch back to main window
        /// </summary>
        public void SwitchToMainWindow()
        {
            //Before switching to main window, wait for current window to close
            WebDriverWait wait = new WebDriverWait(Driver, Constant.WAIT_FOR_EXIST_SEC);
            if (wait.Until<bool>((d) => { return Driver.WindowHandles.Count == 1; }))
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());

            if (Driver.WindowHandles.Contains(mainWindowHandler))
            {
                Driver.SwitchTo().Window(mainWindowHandler);
            }
            else
            {
                Assert.Fail("main open window not available");
            }
        }

        public void TakeScreenShot(string imageName)
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            var data = js.ExecuteScript("return MagicGlass.UI.TestHooks.saveScreenshot('" + imageName + "')");
        }


        public void WaitForClickable(By searchBy, bool byClassName = false)
        {
            //wait for a UI Element to be found
            WebDriverWait wait = new WebDriverWait(Driver, Constant.WAIT_FOR_EXIST_SEC);
            wait.Until(ExpectedConditions.ElementToBeClickable(searchBy));

            //Wait for clickable not working for classname, added sleep for that. 
            //Bug 8087 : Selenium ExpectedConditions.ElementToBeClickable is not working for ClassName
            if (byClassName)
                Thread.Sleep(5000);
        }

        public void WaitForIsVisible(By searchBy)
        {
            //wait for a UI Element to be found
            WebDriverWait wait = new WebDriverWait(Driver, Constant.WAIT_FOR_EXIST_SEC);
            wait.Until(ExpectedConditions.ElementIsVisible(searchBy));
        }
    

        public void Quit()
        {
            this.Driver.Quit();
        }
        
        public void ClickDisplayedElement(IWebElement control, By searchby)
        {
            var allElements = control.FindElements(searchby);
            foreach (var element in allElements)
            {
                if (element.Displayed == true)
                {
                    element.Click();
                    break;
                }
            }
        }
    }
}