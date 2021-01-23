// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static void AddVanirConfiguration(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("vanir.json", true);
            builder.AddEnvironmentVariables();

            var root = builder.Build();
            var config = root.Get<Dolittle.Vanir.Backend.Config.Configuration>();
            services.AddSingleton(config);

        }
    }
}
