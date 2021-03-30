// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend;
using Dolittle.Vanir.Backend.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VanirServiceCollectionExtension
    {
        public static void AddVanir(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IContainer), typeof(Container), ServiceLifetime.Singleton));
            var types = new Types();
            services.Add(new ServiceDescriptor(typeof(ITypes), types));

            var configuration = services.AddVanirConfiguration();
            services.AddGraphQL(types);

            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddResources();
        }
    }
}
