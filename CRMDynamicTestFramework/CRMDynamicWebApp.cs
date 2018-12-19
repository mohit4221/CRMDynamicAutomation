using System;
using CRMDynamicTestFramework.WebDriver;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace CRMDynamicTestFramework
{
    public class CRMDynamicWebApp
    {
        private DriverExtension driverExtension;
        private AppType currentEnvironment;
        private string mainWindowHandler;


        public string MainWindowHandler
        {
            get { return this.mainWindowHandler; }
        }

        public AppType CurrentRunningEnvironment
        {
            get { return this.currentEnvironment; }
        }

        public CRMDynamicWebApp()
        {

        }

        public void StartApplication(string url, AppType environment)
        {
            this.driverExtension = new ChromeDriverExtension(url);
            this.driverExtension.CurrentRunningEnvironment = environment;
            this.mainWindowHandler = this.driverExtension.CurrentWindowHandle;
        }

        public string ExecuteScript(string script)
        {
            return this.driverExtension.ExecuteScript(script);
        }

        public IWebElement FindElementByClassName(string classNameToFind, bool isClickable = false)
        {
            return this.driverExtension.Search(By.ClassName(classNameToFind));
        }
        
        public IWebElement FindElementById(string idToFind, bool isClickable = false)
        {
            return this.driverExtension.Search(By.Id(idToFind));
        }

        public IWebElement FindElementByXpath(string xpathToFind, bool isClickable = false)
        {
            return this.driverExtension.Search(By.XPath(xpathToFind));
        }

        public void CloseBrowserDriver()
        {
            this.driverExtension.Quit();
        }
    }
}
