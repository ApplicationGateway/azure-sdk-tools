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


using System.Management.Automation;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.WindowsAzure.Management.Network.Models;

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS
{
    [Cmdlet(VerbsCommon.Set, "AzureVNetGateway", DefaultParameterSetName = "Connect"), OutputType(typeof(ManagementOperationContext))]
    public class SetAzureVNetGatewayCommand : ServiceManagementBaseCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Connect", HelpMessage = "Connect to Gateway")]
        public SwitchParameter Connect
        {
            get;
            set;
        }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Disconnect", HelpMessage = "Disconnect from Gateway")]
        public SwitchParameter Disconnect
        {
            get;
            set;
        }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "Virtual network name.")]
        public string VNetName
        {
            get;
            set;
        }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "Local Site network name.")]
        public string LocalNetworkSiteName
        {
            get;
            set;
        }

        protected override void OnProcessRecord()
        {
            ServiceManagementProfile.Initialize();

            var connParams = new GatewayConnectDisconnectOrTestParameters
            {
                Operation = this.Connect.IsPresent ? GatewayConnectionUpdateOperation.Connect : GatewayConnectionUpdateOperation.Disconnect
            };

            this.ExecuteClientActionNewSM(
                null,
                this.CommandRuntime.ToString(),
                () => this.NetworkClient.Gateways.ConnectDisconnectOrTest(this.VNetName, this.LocalNetworkSiteName, connParams));
        }
    }
}
