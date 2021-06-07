// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.SDK.Tenancy;
using Dolittle.Vanir.Backend.Execution;
using Dolittle.Vanir.Backend.Resources;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ResourcesServiceCollectionExtensions
    {
        readonly static Dictionary<TenantId, IMongoDatabase> _mongoDatabaseByTenant = new();

        public static void AddResources(this IServiceCollection services, BackendArguments arguments)
        {
            var conventionPack = new ConventionPack {
                    new CamelCaseElementNameConvention()
                };
            ConventionRegistry.Register("camelCase", conventionPack, _ => true);

            var resourceConfigurations = new ResourceConfigurations();
            services.Add(new ServiceDescriptor(typeof(ResourceConfigurations), resourceConfigurations));
            services.Add(new ServiceDescriptor(typeof(IMongoDatabase), (_) =>
            {
                var tenant = ExecutionContextManager.GetCurrent().Tenant;
                if (_mongoDatabaseByTenant.ContainsKey(tenant))
                {
                    return _mongoDatabaseByTenant[tenant];
                }

                var config = resourceConfigurations.GetFor<MongoDbReadModelsConfiguration>(tenant);
                var url = MongoUrl.Create(config.Host);
                var settings = MongoClientSettings.FromUrl(url);
                settings.GuidRepresentation = GuidRepresentation.Standard;
                arguments?.MongoClientSettingsCallback(settings);
                var client = new MongoClient(settings.Freeze());
                var database = client.GetDatabase(config.Database);
                _mongoDatabaseByTenant[tenant] = database;
                return database;
            }, ServiceLifetime.Transient));
        }
    }
}
