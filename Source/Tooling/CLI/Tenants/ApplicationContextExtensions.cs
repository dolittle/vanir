// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dolittle.Vanir.CLI.Contexts;
using Newtonsoft.Json;

namespace Dolittle.Vanir.CLI.Tenants
{
    public static class ApplicationContextExtensions
    {
        public static Guid[] GetTenants(this ApplicationContext context)
        {
            var tenantFiles = new string[] {
                Path.Combine(context.DolittleFolder, "tenants.json")
            };

            foreach (var file in tenantFiles)
            {
                if (File.Exists(file))
                {
                    var json = File.ReadAllText(file);
                    return JsonConvert.DeserializeObject<Dictionary<Guid, object>>(json).Select(kvp => kvp.Key).ToArray();
                }
            }

            return Array.Empty<Guid>();
        }

        public static void SaveTenants(this ApplicationContext context, Guid[] tenants)
        {
            var tenantsFile = Path.Combine(context.DolittleFolder, "tenants.json");
            var dictionary = tenants.ToDictionary(_ => _, _ => new object());
            var json = JsonConvert.SerializeObject(dictionary, SerializerSettings.Default);
            File.WriteAllText(tenantsFile, json);
        }
    }
}
