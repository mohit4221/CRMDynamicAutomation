using CRMDynamicTestFramework.WebDriver;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace CRMDynamicTestFramework
{
    public class CRMDynamicWebApp
    {
        private DriverExtension driverExtension;
        private AppType _currentEnvironment;
        private string _mainWindowHandler;


        public string MainWindowHandler
        {
            get { return this._mainWindowHandler; }
        }

        public AppType CurrentRunningEnvironment
        {
            get { return this._currentEnvironment; }
        }

        public CRMDynamicWebApp()
        {

        }

        public void StartApplication(string url, AppType environment)
        {
            this.driverExtension = new ChromeDriverExtension(url);
        }

        public IWebElement FindElementByClassName(string className, bool isClickable = false)
        {
            return this.driverExtension.FindElementByClassName(className, isClickable);
        }
        public string GetValueFromEmail(string reg, string content)
        {
            Regex regex = new Regex(reg);
            Match match = regex.Match(content);
            if (match.Success)
            {
                return match.Value;
            }
            return null;
        }
        public IWebElement FindElementById(string Id, bool isClickable = false)
        {
            return this.driverExtension.FindElementById(Id, isClickable);
        }

        public IWebElement FindElementByXpath(string xpath, bool isClickable = false)
        {
            return this.driverExtension.FindElementByXpath(xpath, isClickable);
        }

        public void TakeScreenShot(string imageName)
        {
            this.driverExtension.TakeScreenShot(imageName);
        }

        public void WaitForClickable(By by)
        {
            this.driverExtension.WaitForClickable(by);
        }

        public void CloseBrowserDriver()
        {
            this.driverExtension.Quit();
        }
    }
}
