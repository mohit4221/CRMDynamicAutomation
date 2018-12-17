using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CRMDynamicTestFramework.WebDriver
{
    class ChromeDriverExtension : DriverExtension
    {
        private IWebDriver browserWebDriver;

        public override IWebDriver Driver
        {
            get { return this.browserWebDriver; }
        }

        public ChromeDriverExtension(string url)
        {
            this.browserWebDriver = new ChromeDriver(Constant.ChromeDriverPath);
            this.Driver.Manage().Window.Maximize();
            browserWebDriver.Navigate().GoToUrl(url);
        }
    }
}
