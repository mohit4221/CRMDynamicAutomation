using CRMDynamicTestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace CRMDynamicTestCommon
{
    public class CRMDynamicBaseTest
    {
        private CRMDynamicWebApp _dynamicWebApp;
        private AppType _currentEnvironment;

        public CRMDynamicWebApp CRMDynamicWebApp
        {
            get { return this._dynamicWebApp; }
        }

        public CRMDynamicBaseTest()
        {
            _currentEnvironment = AppType.Web;
        }

        public CRMDynamicBaseTest(AppType environment)
        {
            _currentEnvironment = environment;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            try
            {
                _dynamicWebApp = new CRMDynamicWebApp();
                _dynamicWebApp.StartApplication("https://www.google.com", _currentEnvironment);
            }
            catch(Exception ex)
            {
                Assert.Fail(string.Format("Failed to start Application. Error: {0}", ex.Message));
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dynamicWebApp.CloseBrowserDriver();
        }

    }
}
