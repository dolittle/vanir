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
                    Features = features.ToDefinitions().ToArray()
                };

                await sender.SendAsync("newFeatures", notification).ConfigureAwait(false);
            });
        }

        private Features _features = new();

        public FeaturesSubscriptionsResolver(IFeaturesProvider featuresProvider)
        {
            featuresProvider.Features.Subscribe(_ => _features = _);
        }

        public FeatureNotification Features()
        {
            return new FeatureNotification
            {
                Features = _features.ToDefinitions().ToArray()
            };
        }

        [Subscribe(MessageType = typeof(FeatureNotification))]
        [Topic("newFeatures")]
#pragma warning disable CA1707
        public Task<FeatureNotification> system_newFeatures([EventMessage] FeatureNotification notification) => Task.FromResult(notification);
#pragma warning restore
    }
}
