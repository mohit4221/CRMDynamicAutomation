using CRMDynamicTestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace CRMDynamicTestCommon
{
    public class CRMDynamicBaseTest
    {
        private CRMDynamicWebApp dynamicWebApp;
        private AppType currentAppType;

        public CRMDynamicWebApp CRMDynamicWebApp
        {
            get { return this.dynamicWebApp; }
        }

        public CRMDynamicBaseTest()
        {
            currentAppType = AppType.Web;
        }

        public CRMDynamicBaseTest(AppType environment)
        {
            currentAppType = environment;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            try
            {
                dynamicWebApp = new CRMDynamicWebApp();
                dynamicWebApp.StartApplication("https://msretailstore-testing.crm.dynamics.com/main.aspx", currentAppType);
                dynamicWebApp.FindElementById(Constant.MicrosoftEmailText).EnterText("v-sinmo@microsoft.com");
                dynamicWebApp.FindElementById(Constant.MicrosoftNextButton).Clicks();
                //WaitHelper.WaitForPageReady(
                //d =>
                //{
                //    var result = dynamicWebApp.ExecuteScript("return document.readyState");
                //    return result != null && result.Equals("complete");
                //}, ApplicationConfig.SleepIntervalWaitTime);
            }
            catch(Exception ex)
            {
                Assert.Fail(string.Format("Failed to start Application. Error: {0}", ex.Message));
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            dynamicWebApp.CloseBrowserDriver();
        }



    }
}
