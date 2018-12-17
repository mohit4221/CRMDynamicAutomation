// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Common.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json.Linq;
using Quobject.Collections.Immutable;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;

namespace DataFlowTests
{
    /// <summary>
    /// Common class contains all helper functions for unit tests
    /// </summary>
    static public class Common
    {
        /// <summary>
        /// Creates options for DataFlow service
        /// </summary>
        /// <returns> IO.Options </returns>
        public static IO.Options CreateOptions()
        {
            var options = new IO.Options();
            options.ForceNew = true;

            options.Transports = ImmutableList<string>.Empty.Add("websocket");
            return options;
        }

        /// <summary>
        /// Creates JObject for DFS logon api
        /// </summary>
        /// <param name="source">Identity service response object</param>
        /// <returns>JObject</returns>
        public static JObject GetLogonObject(JObject source, string deviceId)
        {
            JObject logonObject = new JObject();
            logonObject["userId"] = (string)source["prn"];
            logonObject["deviceId"] = deviceId;
            logonObject["token"] = (string)source["token"];

            return logonObject;
        }
    }
}