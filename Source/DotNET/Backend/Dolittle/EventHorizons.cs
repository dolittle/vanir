// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using Dolittle.SDK.Tenancy;
using Newtonsoft.Json;

namespace Dolittle.Vanir.Backend.Dolittle
{
    public class EventHorizons : Dictionary<TenantId, EventHorizonDefinition[]>
    {
        public static EventHorizons Load()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), ".dolittle", "event-horizons.json");
            if (File.Exists(file))
            {
                var json = File.ReadAllText(file);
                return JsonConvert.DeserializeObject<EventHorizons>(json);
            }
            return new EventHorizons();
        }
    }
}
