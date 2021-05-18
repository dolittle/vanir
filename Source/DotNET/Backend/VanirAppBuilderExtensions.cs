// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    public static class VanirAppBuilderExtensions
    {
        public static IApplicationBuilder UseVanir(this IApplicationBuilder app)
        {
            Dolittle.Vanir.Backend.Container.ServiceProvider = app.ApplicationServices;
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

            app.UseDolittle();
            app.UseGraphQL();
            return app;
        }

        public static IApplicationBuilder UseVanirWithCommon(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger"));
            }
            app
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseVanir()
                .UseRouting()
                .UseWebSockets()

                .UseEndpoints(_ =>
                {
                    _.MapControllers();
                    _.MapDefaultControllerRoute();
                    _.MapGraphQL(app);
                });

            return app;
        }
    }
}
