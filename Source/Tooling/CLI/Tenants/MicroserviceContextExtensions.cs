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

    public static class MicroserviceContextExtensions
    {
        public static Guid[] GetTenants(this MicroserviceContext context)
        {
            var tenantFiles = new string[] {
                Path.Combine(context.DolittleFolder, "tenants.json"),
                Path.Combine(context.Application.DolittleFolder, "tenants.json")
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

        public static void SaveTenants(this MicroserviceContext context, Guid[] tenants)
        {
            var microserviceTenants = Path.Combine(context.DolittleFolder, "tenants.json");
            var applicationTenants = Path.Combine(context.Application.DolittleFolder, "tenants.json");
            var file = File.Exists(microserviceTenants) ? microserviceTenants : applicationTenants;
            var dictionary = tenants.ToDictionary(_ => _, _ => new object());
            var json = JsonConvert.SerializeObject(dictionary, SerializerSettings.Default);
            File.WriteAllText(file, json);
        }
    }
}
