// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dolittle.Vanir.Backend.Features
{
    public class FeaturesSubscriptionsResolver
    {
        public static void Initialize(IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetService<IFeaturesProvider>();
            var sender = app.ApplicationServices.GetService<ITopicEventSender>();

            provider.Features.Subscribe(async features =>
            {
                var notification = new FeatureNotification
                {
                    Features = features.Select(kvp => new Feature { Name = kvp.Key, IsOn = kvp.Value.IsOn }).ToArray()
                };

                await sender.SendAsync("newFeatures", notification);
            });
        }


        [Subscribe(MessageType = typeof(FeatureNotification))]
        [Topic("newFeatures")]
        public Task<FeatureNotification> NewFeatures([EventMessage] FeatureNotification notification) => Task.FromResult(notification);
    }
}
