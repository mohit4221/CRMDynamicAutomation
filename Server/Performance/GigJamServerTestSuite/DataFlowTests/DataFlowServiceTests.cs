// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataFlowServiceTests.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Socket = Quobject.SocketIoClientDotNet.Client.Socket;

namespace DataFlowTests
{
    [TestClass]
    public class DataFlowServiceTests
    {
        public TestContext TestContext { set; get; }

        private Socket socket;
        private string deviceId = Guid.NewGuid().ToString();
        private ConnectionStringSettingsCollection appSettings = ConfigurationManager.ConnectionStrings;
        private JObject logonObject;
        private JObject identityResponseObject;
        private string sessionId;

        #region Test initializations

        /// <summary>
        /// Test initialization methods. Each test has its own initialization logic.
        /// </summary>
        [TestInitialize]
        public async void Initialize()
        {
            switch (TestContext.TestName)
            {
                case "DFSLogonTest":
                    await IntializeDFSLogon();
                    break;

                case "CreateSessionTest":
                    await IntializeCreateSessionTest();
                    break;

                case "UpdateSessionTest":
                    await IntializeUpdateSessionTest();
                    break;

                default:
                    break;
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            deviceId = null;
            socket.Close();
        }

        private async Task IntializeDFSLogon()
        {
            ConnectIdentity().Wait();
            ConnectToDFS().Wait();
        }

        private async Task IntializeCreateSessionTest()
        {
            ConnectIdentity().Wait();
            ConnectToDFS().Wait();
            Logon().Wait();
        }

        private async Task IntializeUpdateSessionTest()
        {
            ConnectIdentity().Wait();
            ConnectToDFS().Wait();
            Logon().Wait();
            CreateSession().Wait();
        } 

        #endregion

        #region Common methods
        
        /// <summary>
        /// Connect to Identity service and get response and logon objects
        /// </summary>
        private async Task ConnectIdentity()
        {
            HttpClient httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "username", "testuser1" },
                { "password", "appmagic1!" }
            });

            HttpResponseMessage responseMessage = await httpClient.PostAsync(appSettings["IdentityServiceConnectionString"].ConnectionString, content);
            var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
            
            identityResponseObject = JObject.Parse(jsonResponse);
            logonObject = Common.GetLogonObject(identityResponseObject, deviceId);
        }

        /// <summary>
        /// Connect to DataFlow service
        /// </summary>
        private async Task ConnectToDFS()
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            bool dfsConnected = false;

            var options = Common.CreateOptions();
            socket = IO.Socket(appSettings["DataFlowConnectionString"].ConnectionString, options);
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                dfsConnected = true;
                manualResetEvent.Set();
            });

            socket.On(Socket.EVENT_DISCONNECT, () =>
            {
                Assert.IsNull(deviceId, "Socket was disconnected before test was finished");
            });

            manualResetEvent.WaitOne();
            Assert.IsTrue(dfsConnected, "DFS connection was not established: " + deviceId);
        }

        /// <summary>
        /// Log into DataFlow Service. 
        /// Requirements: Connection to Identity/DataFlow services
        /// </summary>
        private async Task Logon()
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            string logonResult = String.Empty;
            
            socket.Emit("logon", new AckImpl((callback) =>
            {
                JToken token = JObject.Parse(callback.ToString());
                logonResult = (string)token.SelectToken("success");
                manualResetEvent.Set();
            }), logonObject);

            manualResetEvent.WaitOne();

            Assert.AreEqual("True", logonResult, "Logon must return successful result: " + deviceId);
        }

        /// <summary>
        /// Create a gig. 
        /// Requirements: Connection to Identity/DataFlow services and DataFlow log in.
        /// </summary>
        private async Task CreateSession()
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            string sessionUpdateResult = String.Empty;
            string sessionCreateResult = String.Empty;

            JObject sessionObject = new JObject();
            sessionObject["userId"] = (string)identityResponseObject["prn"];
            sessionObject["appId"] = Guid.NewGuid().ToString();
            sessionObject["name"] = "OldName";

            socket.Emit("createSession", ((callback) =>
            {
                JToken token = JObject.Parse(callback.ToString());
                sessionId = (string)token.SelectToken("sessionId");
                sessionCreateResult = (string)token.SelectToken("success");
                manualResetEvent.Set();
            }), sessionObject);

            manualResetEvent.WaitOne();

            Assert.AreEqual("True", sessionCreateResult, "Session was not created: " + deviceId);
            Assert.IsFalse(String.IsNullOrEmpty(sessionId), "Session id was empty: " + deviceId);
        }

        #endregion

        #region Test Methods
        
        [TestMethod]
        public async Task DFSLogonTest()
        {
            await Logon();
        }

        [TestMethod]
        public async Task CreateSessionTest()
        {
            await CreateSession();
        }

        [TestMethod]
        public async Task UpdateSessionTest()
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            string sessionUpdateResult = String.Empty;
            
            JObject sessionUpdateObject = new JObject();
            sessionUpdateObject["sessionId"] = sessionId;
            sessionUpdateObject["name"] = "NewName";

            manualResetEvent.Reset();
            socket.Emit("updateSession", ((callback2) =>
            {
                JToken token = JObject.Parse(callback2.ToString());
                sessionUpdateResult = (string)token.SelectToken("success");
                manualResetEvent.Set();
            }), sessionUpdateObject);

            manualResetEvent.WaitOne();
            Assert.AreEqual("True", sessionUpdateResult, "Session was not updated: " + deviceId);
        }

        #endregion
    }
}

