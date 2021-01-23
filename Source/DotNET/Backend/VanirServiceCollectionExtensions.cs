// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VanirServiceCollectionExtension
    {
        public static void AddVanir(this IServiceCollection services)
        {
            services.AddGraphQL(_ => _.EnableMetrics = true)
                .AddErrorInfoProvider(_ => _.ExposeExceptionStackTrace = true)
                .AddNewtonsoftJson()
                .AddGraphTypes();
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddVanirConfiguration();
        }
    }
}
