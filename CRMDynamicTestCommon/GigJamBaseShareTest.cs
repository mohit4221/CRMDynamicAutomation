using CRMDynamicTestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenQA.Selenium;

namespace GigJamTestCommon
{
    public class GigJamBaseShareTest
    {
        private CRMDynamicApp _app1;
        private CRMDynamicApp _app2;
        private CRMDynamicApp _app3;
        private CRMDynamicApp _app4;
        private CRMDynamicApp _app5;

        public CRMDynamicApp CRMDynamicApp1
        {
            get { return this._app1; }
        }

        public CRMDynamicApp CRMDynamicApp2
        {
            get { return this._app2; }
        }
        public CRMDynamicApp CRMDynamicApp3
        {
            get { return this._app3; }
        }
        public CRMDynamicApp CRMDynamicApp4
        {
            get { return this._app4; }
        }
        public CRMDynamicApp CRMDynamicApp5
        {
            get { return this._app5; }
        }

        public void TestInitialize(string url)
        {
            try
            {
                //_app1 = new CRMDynamicApp();
                //_app1.StartApplication(url, OSType.Windows, Constant.cacheName1);

                _app1 = new CRMDynamicApp();
              //  _app1.StartApplication(url, OSType.Windows, Constant.cacheName2);

                //_app3 = new CRMDynamicApp();
                //_app3.StartApplication(url, OSType.Windows, Constant.cacheName1);

                //_app4 = new CRMDynamicApp();
                //_app4.StartApplication("https://www.mygov.in/group-issue/saharanpur-smart-city-proposal-round-2/", OSType.Windows, Constant.cacheName1);

                //_app5 = new CRMDynamicApp();
                //_app5.StartApplication(url, OSType.Windows, Constant.cacheName1);
            }
            catch (Exception ex)
            {
                ApplicationHelper.CloseAllRunningInstance();
                Assert.Fail(string.Format("Failed to start Application. Error: ", ex.Message));
            }
        }

        public void OpenNewWindow(string url)
        {
            try
            {
                _app3 = new CRMDynamicApp();
                _app3.StartApplication(url, OSType.Windows, Constant.cacheName1);
            }
            catch (Exception ex)
            {
                ApplicationHelper.CloseAllRunningInstance();
                Assert.Fail(string.Format("Failed to start Application. Error: ", ex.Message));
            }
        }
        //    [TestCleanup]
        //    public void TestCleanup()
        //    {
        //        _app1.StopApplication();
        //        _app2.StopApplication();
        //        _app3.StopApplication();
        //        ApplicationHelper.CloseAllRunningInstance();
        //    }
    }
}
