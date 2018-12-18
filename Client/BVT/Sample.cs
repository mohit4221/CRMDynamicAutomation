using CRMDynamicTestCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CRMDynamicTestFramework;

namespace CRMDynamicClientTest.BVT
{
    [TestClass]
    public class Sample : CRMDynamicBaseTest
    {
        [TestCategory("BVT"), TestMethod()]
        public void TestMethod1()
        {
            try
            {
                CRMDynamicWebApp.FindElementById(Constant.MicrosoftEmailText).EnterText("v-sinmo@microsoft.com");
                CRMDynamicWebApp.FindElementById(Constant.MicrosoftNextButton).Clicks();

            }
            catch (Exception e)
            {
                Assert.Fail("Failed with Exception: " + e.Message);
            }
        }
    }
}
