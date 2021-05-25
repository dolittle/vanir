// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Features;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> for adding Features services.
    /// </summary>
    public static class FeaturesServiceCollectionExtensions
    {
        /// <summary>
        /// Add Features services.
        /// </summary>
        /// <param name="services"><see cref="IServiceColletion"/> to add to.</param>
        public static void AddFeatures(this IServiceCollection services)
        {
            services.AddSingleton<IFeaturesProvider, FeaturesProvider>();
            services.AddSingleton<IFeatureToggles, FeatureToggles>();
            services.AddSingleton<IFeaturesParser, FeaturesParser>();
        }
    }
}
