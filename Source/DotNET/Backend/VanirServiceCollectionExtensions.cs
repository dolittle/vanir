// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using GraphQL.AspNet.Configuration.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class VanirServiceCollectionExtension
    {
        public static void AddVanir(this IServiceCollection services)
        {
            var configuration = services.AddVanirConfiguration();

            var schemaOptions = services.AddGraphQL(_ =>
            {
                _.ExecutionOptions.EnableMetrics = true;
                _.QueryHandler.Route = configuration.GraphQLRoute;
            });

            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddResources();
        }
    }
}
