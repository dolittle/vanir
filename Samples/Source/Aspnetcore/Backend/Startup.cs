using System;
using Autofac;
using Dolittle.SDK;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Backend
{

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog(Log.Logger);

            services.AddVanir(new()
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
            app.UseVanir();
        }
    }
}
