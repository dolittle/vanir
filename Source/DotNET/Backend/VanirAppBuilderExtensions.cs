// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using GraphQL.AspNet.Configuration.Mvc;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    public static class VanirAppBuilderExtensions
    {
        public static void UseVanir(this IApplicationBuilder app)
        {
            app.UseVanirCommon();
            app.UseGraphQL();
        }

        public static void UseVanir<TSchema>(this IApplicationBuilder app)
            where TSchema : Schema
        {
            app.UseVanirCommon();
            app.UseGraphQL<TSchema>();
        }

        private static void UseVanirCommon(this IApplicationBuilder app)
        {
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
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());

            app.UseEndpoints(_ =>
            {
                _.MapControllers();
                _.MapDefaultControllerRoute();
            });
        }
    }
}
