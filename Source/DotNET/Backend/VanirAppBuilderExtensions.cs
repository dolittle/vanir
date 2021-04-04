// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Config;
using GraphQL.Server.Ui.Playground;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{

    public static class VanirAppBuilderExtensions
    {
        public static void UseVanir(this IApplicationBuilder app)
        {
            app.UseVanirCommon();
        }

        static void UseVanirCommon(this IApplicationBuilder app)
        {
            Dolittle.Vanir.Backend.Container.ServiceProvider = app.ApplicationServices;
            Dolittle.Vanir.Backend.Dolittle.DolittleContainer.ServiceProvider = app.ApplicationServices;

            var logger = app.ApplicationServices.GetService<ILogger<Dolittle.Vanir.Backend.Vanir>>();
            var configuration = app.ApplicationServices.GetService<Configuration>();
            var prefix = configuration.Prefix;

            if (prefix.Length > 0)
            {
                logger.LogInformation($"Using '{prefix}' as prefix");
            }
            else
            {
                logger.LogInformation("Using no prefix");
            }

            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger"));
            }

            app.UseExecutionContext();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value == configuration.GraphQLRoute && !context.Request.HasJsonContentType())
                {
                    logger.LogInformation("Hello there");
                    context.Request.Path = configuration.GraphQLPlaygroundRoute;
                }
                await next();
            });

            app.UseEndpoints(_ =>
            {
                _.MapControllers();
                if (env.IsDevelopment())
                {
                    logger.LogInformation($"Hosting Playground at '{configuration.GraphQLPlaygroundRoute}'");
                    _.MapGraphQLPlayground(new PlaygroundOptions { GraphQLEndPoint = configuration.GraphQLRoute }, configuration.GraphQLPlaygroundRoute);
                }


                logger.LogInformation($"GraphQL endpoint is located at '{configuration.GraphQLRoute}'");
                _.MapGraphQL(configuration.GraphQLRoute).WithOptions(new GraphQLServerOptions
                {
                    Tool = { Enable = false }
                });

                _.MapDefaultControllerRoute();
            });

            app.UseDolittle();
        }
    }
}
