// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend;
using Dolittle.Vanir.Backend.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VanirServiceCollectionExtension
    {
        public static Services AddVanir(this IServiceCollection services, BackendArguments arguments = null)
        {
            var container = new Container();
            services.AddSingleton<IContainer>(container);
            var types = new Types();
            services.Add(new ServiceDescriptor(typeof(ITypes), types));

            var configuration = services.AddVanirConfiguration();
            services.AddDolittle(configuration, arguments, types);
            services.AddExecutionContext();
            services.AddGraphQL(container, arguments, types);

            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddResources(arguments);

            return new Services { Types = types };
        }
    }
}
