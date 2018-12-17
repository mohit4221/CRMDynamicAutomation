using CRMDynamicTestCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace GigJamClientTest
{
    [TestClass]
    public class ClientBVT : CRMDynamicBaseTest
    {
        /// <summary>
        /// Add control test
        /// </summary>
        [TestCategory("BVT"),TestMethod()]
        public void AddControl()
        {
            try
            {
                
            }
            catch (Exception e)
            {
                Assert.Fail("Failed with Exception: " + e.Message);
            }
        }

    
    }

}
