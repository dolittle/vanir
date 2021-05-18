using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Dolittle.SDK;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Backend
{
    public class MyPolicy : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return Task.FromResult(AuthorizationResult.Failed());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            return Task.FromResult(AuthorizationResult.Failed());
        }
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog(Log.Logger);

            services.AddAuthorization(options => options
                .AddPolicy("MyPolicy", policy =>
                 policy.RequireClaim("Admin")));

            services.AddVanirWithCommon(new()
            {
                LoggerFactory = loggerFactory,
                GraphQLExecutorBuilder = (IRequestExecutorBuilder _) =>
                {
                    _.BindRuntimeType<UserId, StringType>();
                    _.AddTypeConverter<string, UserId>(input => new UserId { Value = Guid.Parse("2c6b9328-93b6-4c84-9523-9a7b2b745f64") });
                },
                DolittleClientBuilderCallback = (ClientBuilder _) =>
                {
                }
            });
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyModules(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseVanirWithCommon();
        }
    }
}
