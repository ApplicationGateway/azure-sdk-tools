﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Commands.Common;
using Microsoft.WindowsAzure.Commands.Common.Models;
using Microsoft.WindowsAzure.Commands.Common.Test.Common;
using Microsoft.WindowsAzure.Commands.Common.Test.Mocks;

namespace Microsoft.WindowsAzure.Commands.Test.Utilities.Common
{
    /// <summary>
    /// Base class for Microsoft Azure PowerShell unit tests.
    /// </summary>
    public abstract class TestBase
    {
        public TestBase()
        {
            TestingTracingInterceptor.AddToContext();
            ProfileClient.DataStore = new MockDataStore();
            AzureSession.SetCurrentSubscription(new AzureSubscription {Id = Guid.NewGuid(), Name = "test"}, null);
        }
        /// <summary>
        /// Gets or sets a reference to the TestContext used for interacting
        /// with the test framework.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Log a message with the test framework.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="args">Arguments.</param>
        public void Log(string format, params object[] args)
        {
            Debug.Assert(TestContext != null);
            TestContext.WriteLine(format, args);
        }
    }
}
