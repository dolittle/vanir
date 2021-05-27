// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using Dolittle.Vanir.CLI.Contexts;
using Newtonsoft.Json;

namespace Dolittle.Vanir.CLI.EventHorizon
{
    public static class MicroserviceContextExtensions
    {
        public static EventHorizonConsents GetEventHorizonConsents(this MicroserviceContext context)
        {
            var file = Path.Combine(context.DolittleFolder, "event-horizon-consents.json");
            if (!File.Exists(file)) return new EventHorizonConsents();

            var json = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<EventHorizonConsents>(json);
        }

        public static void SaveEventHorizonConsents(this MicroserviceContext context, EventHorizonConsents consents)
        {
            var file = Path.Combine(context.DolittleFolder, "event-horizon-consents.json");
            var json = JsonConvert.SerializeObject(consents, SerializerSettings.Default);
            File.WriteAllText(file, json);
        }
    }
}
