// ----------------------------------------------------------------------------------
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

namespace Microsoft.WindowsAzure.Commands.SqlDatabase.Database.Cmdlet
{
    using System;
    using System.Management.Automation;
    using Commands.Utilities.Common;
    using Properties;
    using Services.Common;
    using Services.Server;

    /// <summary>
    /// Retrieves a list of Windows Azure SQL Databases in the given server context.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSqlDatabase", ConfirmImpact = ConfirmImpact.None,
        DefaultParameterSetName = ByConnectionContext)]
    public class GetAzureSqlDatabase : GetAzureSqlDatabaseBase
    {
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }

        protected override void OperationOnContext(IServerDataServiceContext context, string databaseName)
        {
            this.WriteObject(context.GetDatabase(databaseName), true);
        }

        protected override void OperationOnContext(IServerDataServiceContext context)
        {
            // ximchen Mark: Need to put a true here otherwise pass to next pipeline won't work
            this.WriteObject(context.GetDatabases(), true);
        }
    }
}
