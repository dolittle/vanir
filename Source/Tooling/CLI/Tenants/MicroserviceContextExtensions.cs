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
            var file = Path.Combine(context.DolittleFolder, "tenants.json");
            if (!File.Exists(file)) return Array.Empty<Guid>();

            var json = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Dictionary<Guid, object>>(json).Select(kvp => kvp.Key).ToArray();
        }

        public static void SaveTenants(this MicroserviceContext context, Guid[] tenants)
        {
            var file = Path.Combine(context.DolittleFolder, "tenants.json");
            var dictionary = tenants.ToDictionary(_ => _, _ => new object());
            var json = JsonConvert.SerializeObject(dictionary, SerializerSettings.Default);
            File.WriteAllText(file, json);
        }
    }
}
