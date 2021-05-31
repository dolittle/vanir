// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Dolittle.Vanir.Backend.Config;
using Dolittle.Vanir.Backend.Features;
using GraphQL.Server.Ui.Playground;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    public static class GraphQLAppBuilderExtensions
    {
        public static IApplicationBuilder UseGraphQL(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            var logger = app.ApplicationServices.GetService<ILogger<Dolittle.Vanir.Backend.Vanir>>();
            var configuration = app.ApplicationServices.GetService<Configuration>();

            app.Use(async (context, next) =>
            {
                if (!context.WebSockets.IsWebSocketRequest &&
                    !context.Request.Headers.ContainsKey("Upgrade") &&
                    !context.Request.Headers["Upgrade"].ToArray().Any(_ => _.Equals("websocket", StringComparison.InvariantCultureIgnoreCase)) &&
                    context.Request.Path.Value == configuration.GraphQLRoute && !context.Request.HasJsonContentType())
                {
                    context.Request.Path = configuration.GraphQLPlaygroundRoute;
                }
                await next().ConfigureAwait(false);
            });

            FeaturesSubscriptionsResolver.Initialize(app);

            return app;
        }
    }

    public static class GraphQLEndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapGraphQL(this IEndpointRouteBuilder endpoints, IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            var logger = app.ApplicationServices.GetService<ILogger<Dolittle.Vanir.Backend.Vanir>>();
            var configuration = app.ApplicationServices.GetService<Configuration>();

            if (env.IsDevelopment())
            {
                logger.LogInformation($"Hosting Playground at '{configuration.GraphQLPlaygroundRoute}'");
                endpoints.MapGraphQLPlayground(
                    new PlaygroundOptions
                    {
                        GraphQLEndPoint = configuration.GraphQLRoute,
                        SubscriptionsEndPoint = configuration.GraphQLRoute
                    }, configuration.GraphQLPlaygroundRoute);
            }

            logger.LogInformation($"GraphQL endpoint is located at '{configuration.GraphQLRoute}'");

            endpoints.MapGraphQL(configuration.GraphQLRoute).WithOptions(new GraphQLServerOptions
            {
                Tool = { Enable = false }
            });

            return endpoints;
        }
    }
}
