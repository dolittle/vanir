// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend;
using Dolittle.Vanir.Backend.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VanirServiceCollectionExtension
    {
        /// <summary>
        /// Add the basic Vanir setup
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to extend.</param>
        /// <param name="arguments"><see cref="BackendArguments"/> if any - defaults to default values if not provided.</param>
        /// <returns><see cref="Services"/></returns>
        /// <remarks>
        /// This configures GraphQL, Dolittle, MongoDB and multi tenant resource management according
        /// to the requirement of the Dolittle platform (https://dolittle.io/docs/platform/requirements/).
        /// </remarks>
        public static Services AddVanir(this IServiceCollection services, BackendArguments arguments = null)
        {
            if (arguments == null) arguments = new();

            var container = new Container();
            services.AddSingleton<IContainer>(container);
            var types = new Types();
            services.Add(new ServiceDescriptor(typeof(ITypes), types));

            var configuration = services.AddVanirConfiguration();
            services.AddDolittle(configuration, arguments, types);
            services.AddFeatures();
            services.AddGraphQL(container, arguments, types);

            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddMongoDB();
            services.AddResources(arguments);

            return new Services
            {
                Configuration = configuration,
                Container = container,
                Types = types
            };
        }

        /// <summary>
        /// Add the basic Vanir setup with additional common setup.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to extend.</param>
        /// <param name="arguments"><see cref="BackendArguments"/> if any - defaults to default values if not provided.</param>
        /// <returns><see cref="Services"/></returns>
        /// <remarks>
        /// This configures GraphQL, Dolittle, MongoDB and multi tenant resource management according
        /// to the requirement of the Dolittle platform (https://dolittle.io/docs/platform/requirements/).
        /// </remarks>
        public static Services AddVanirWithCommon(this IServiceCollection services, BackendArguments arguments = null)
        {
            var result = services.AddVanir(arguments);
            services.AddControllers();
            services.AddSwaggerGen();
            return result;
        }
    }
}
