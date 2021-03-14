# Available Dependencies

With the [IoC](./ioc.md) that is configured, during startup of the [host](./host.md) there are a few dependencies
that gets hooked up that can be used directly in your code.

| Type | Import Location | Description |
| ---- | --------------- | ----------- |
| Configuration | @dolittle/vanir-backend | The configuration of the microservice |
| IEventStore | @dolittle/vanir-backend | The Dolittle event store - scoped correctly for the current tenant |
| IEventTypes | @dolittle/vanir-backend | The event types registered with the Dolittle runtime |
| IMongoDatabase | @dolittle/vanir-backend | The mongo database wrapper - scoped correctly for the current tenant |
| MongoDbReadModelsConfiguration | @dolittle/vanir-backend | The MongoDb configuration - scoped correctly for the current tenant |
| MongoDatabaseProvider | @dolittle/vanir-backend | Provider for getting instances of IMongoDatabase for specific contexts |
| IResourceConfigurations | @dolittle/vanir-backend | The resource configurations for all configured resources and tenants |
| MongoClient | mongodb | The official mongo client - scoped and configured correctly for the current tenant |
| IContainer | @dolittle/vanir-dependency-inversion | The IoC container wrapper - typically for getting instances |
| Client | @dolittle/sdk | The configured Dolittle client |
