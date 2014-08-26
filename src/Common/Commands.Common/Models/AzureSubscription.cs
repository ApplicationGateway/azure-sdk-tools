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
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Commands.Common.Models
{
    public class AzureSubscription
    {
        public AzureSubscription()
        {
            Properties = new Dictionary<Property,string>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Environment { get; set; }

        public Dictionary<Property, string> Properties { get; set; }

        public enum Property
        {
            /// <summary>
            /// Comma separated registered resource providers, i.e.: websites,compute,hdinsight
            /// </summary>
            RegisteredResourceProviders,

            /// <summary>
            /// Comma separated mode names that this subscription supports, i.e.: AzureResourceManager,AzureServiceManagement
            /// </summary>
            SupportedModes,

            /// <summary>
            /// If this property existed on the subscription indicates that it's default one.
            /// </summary>
            Default,

            CloudStorageAccount,

            DefaultPrincipalName,

            AvailablePrincipalNames,

            Thumbprint
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public string GetProperty(Property property)
        {
            if (Properties.ContainsKey(property))
            {
                return Properties[property];
            }

            return null;
        }

        public string[] GetPropertyAsArray(Property property)
        {
            if (Properties.ContainsKey(property))
            {
                return Properties[property].Split(',');
            }

            return new string[0];
        }

        public void SetProperty(Property property, params string[] values)
        {
            if (values == null || values.Length == 0)
            {
                if (Properties.ContainsKey(property))
                {
                    Properties.Remove(property);
                }
            }
            else
            {
                Properties[property] = string.Join(",", values);
            }
        }

        public override bool Equals(object obj)
        {
            var anotherSubscription = obj as AzureSubscription;
            if (anotherSubscription == null)
            {
                return false;
            }
            else
            {
                return anotherSubscription.Id == Id;
            }
        }
    }
}
