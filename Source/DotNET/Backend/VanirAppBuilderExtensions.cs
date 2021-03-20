// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend;
using Dolittle.Vanir.Backend.Config;
using GraphQL.AspNet.Configuration.Mvc;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Hosting;
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
            app.UseGraphQL();
        }


        static void UseVanirCommon(this IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetService<ILogger<Vanir>>();
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
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions
            {
                GraphQLEndPoint = $"{configuration.GraphQLRoute}",
                Path = $"{configuration.GraphQLRoute}"
            });

            app.UseEndpoints(_ =>
            {
                _.MapControllers();
                _.MapDefaultControllerRoute();
            });
        }
    }
}
