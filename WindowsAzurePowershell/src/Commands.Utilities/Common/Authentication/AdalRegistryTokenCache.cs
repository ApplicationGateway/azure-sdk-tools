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

namespace Microsoft.WindowsAzure.Commands.Utilities.Common.Authentication
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using IdentityModel.Clients.ActiveDirectory;
    using Win32;

    /// <summary>
    /// An implementation of the Adal token cache that stores the cache items
    /// in the Windows registry.
    /// </summary>
    public class AdalRegistryTokenCache : IDictionary<TokenCacheKey, string>
    {
        private const string hivePath = "Software\\Microsoft\\WindowsAzurePowershell\\TokenCache";
        private readonly IDictionary<string, string> registry;


        public AdalRegistryTokenCache()
        {
            registry = new RegistryBackedDictionary(Registry.CurrentUser, hivePath);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TokenCacheKey, string>> GetEnumerator()
        {
            return registry.Select(ToCacheItem).GetEnumerator();
        }

        public void Add(KeyValuePair<TokenCacheKey, string> item)
        {
            registry.Add(ToRegistryItem(item));
        }

        public void Clear()
        {
            registry.Clear();
        }

        public bool Contains(KeyValuePair<TokenCacheKey, string> item)
        {
            return registry.Contains(ToRegistryItem(item));
        }

        public void CopyTo(KeyValuePair<TokenCacheKey, string>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "less than zero");
            }
            if (arrayIndex + Count > array.Length)
            {
                throw new ArgumentException("No room");
            }

            int index = 0;
            foreach (var kvp in registry)
            {
                array[index++] = ToCacheItem(kvp);
            }
        }

        public bool Remove(KeyValuePair<TokenCacheKey, string> item)
        {
            return registry.Remove(ToRegistryItem(item));
        }

        public int Count { get { return registry.Count; } }
        public bool IsReadOnly { get { return false; } }

        public bool ContainsKey(TokenCacheKey key)
        {
            return registry.ContainsKey(StringFromCacheKey(key));
        }

        public void Add(TokenCacheKey key, string value)
        {
            Add(new KeyValuePair<TokenCacheKey, string>(key, value));
        }

        public bool Remove(TokenCacheKey key)
        {
            return registry.Remove(StringFromCacheKey(key));
        }

        public bool TryGetValue(TokenCacheKey key, out string value)
        {
            return registry.TryGetValue(StringFromCacheKey(key), out value);
        }

        public string this[TokenCacheKey key]
        {
            get { return registry[StringFromCacheKey(key)]; }
            set { registry[StringFromCacheKey(key)] = value; }
        }

        public ICollection<TokenCacheKey> Keys
        {
            get
            {
                return registry.Keys.Select(CacheKeyFromString).ToList();
            }
        }

        public ICollection<string> Values
        {
            get { return registry.Values; }
        }
        
        private TokenCacheKey CacheKeyFromString(string key)
        {
            var fields = key.Split(new[] { "::" }, StringSplitOptions.None);
            for (var i = 0; i < fields.Length; ++i)
            {
                fields[i] = fields[i].Replace("`:", ":");
                fields[i] = fields[i].Replace("``", "`");
            }

            return new TokenCacheKey
            {
                Authority = fields[0],
                ClientId = fields[1],
                ExpiresOn = DateTimeOffset.Parse(fields[2], CultureInfo.InvariantCulture),
                FamilyName = fields[3],
                GivenName = fields[4],
                IdentityProviderName = fields[5],
                IsMultipleResourceRefreshToken = bool.Parse(fields[6]),
                IsUserIdDisplayable = bool.Parse(fields[7]),
                Resource = fields[8],
                TenantId = fields[9],
                UserId = fields[10]
            };
        }

        private string StringFromCacheKey(TokenCacheKey key)
        {
            string[] fields = new[] {
                key.Authority,
                key.ClientId,
                key.ExpiresOn.ToString("zzz", CultureInfo.InvariantCulture),
                key.FamilyName,
                key.GivenName,
                key.IdentityProviderName,
                key.IsMultipleResourceRefreshToken.ToString(),
                key.IsUserIdDisplayable.ToString(),
                key.Resource,
                key.TenantId,
                key.UserId
            };

            // Escape our separator characters. Using ` instead
            // of \ because hey, powershell.
            for (int i = 0; i < fields.Length; ++i)
            {
                fields[i] = fields[i].Replace("`", "``");
                fields[i] = fields[i].Replace(":", "`:");
            }
            return string.Join("::", fields);
        }

        private KeyValuePair<string, string> ToRegistryItem(KeyValuePair<TokenCacheKey, string> item)
        {
            return new KeyValuePair<string, string>(StringFromCacheKey(item.Key), item.Value);
        }

        private KeyValuePair<TokenCacheKey, string> ToCacheItem(KeyValuePair<string, string> item)
        {
            return new KeyValuePair<TokenCacheKey, string>(CacheKeyFromString(item.Key), item.Value);
        }
    }
}