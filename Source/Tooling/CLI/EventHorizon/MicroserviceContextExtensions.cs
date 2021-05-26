// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Dolittle.Vanir.CLI.Contexts;
using Newtonsoft.Json;

namespace Dolittle.Vanir.CLI.EventHorizon
{
    public static class MicroserviceContextExtensions
    {
        public static IDictionary<Guid, EventHorizonConsent> GetEventHorizonConsents(this MicroserviceContext context)
        {
            var file = Path.Combine(context.DolittleFolder, "event-horizon-consents.json");
            if (!File.Exists(file)) return new Dictionary<Guid, EventHorizonConsent>();

            var json = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Dictionary<Guid, EventHorizonConsent>>(json);
        }
    }
}
