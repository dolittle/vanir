# Configuring Startup.cs

Vanir provides flexibility in how you configure your application startup and the goal is for
it to not be obtrusive.

## .AddVanir() / .UseVanir() / .MapGraphQL()

If you want to control of your services and the middleware pipelines, the least opinionated setup
would be to use the `.AddVanir()` and `.UseVanir()` methods in your `Startup` type:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddVanir();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseVanir();

        app.UseRouting();
        app.UseEndpoints(_ =>
        {
            _.MapControllers();
            _.MapDefaultControllerRoute();
            _.MapGraphQL(app);
        });
    }
}
```

### .AddVanir()

With `.AddVanir()` you get a setup that configures the following:

- Dolittle client
- [HotChocolate](https://chillicream.com/docs/hotchocolate/) GraphQL
- MongoDB defaults and serializers
- Resource management according to the [Dolittle platform requirements](https://dolittle.io/docs/platform/requirements/) supporting the [resource.json](https://dolittle.io/docs/reference/runtime/configuration/#resourcesjson) file format.

#### Backend Arguments

Optionally you can provide an instance of a type called `BackendArguments`.
On this type you'll find ways to amend the Dolittle client builder, GraphQL builder and Mongo client settings.
It also provides configuration for controlling the behavior of Vanir:

| Property Name | Description | Default Value |
| ------------- | ----------- | ------------- |
| LoggerFactory | One can provide an instance of logger factory of the type `Microsoft.Extensions.Logging.ILoggerFactory` | NullLoggerFactory |
| GraphQLExecutorBuilder | Callback for building on the [GraphQL builder](https://chillicream.com/docs/hotchocolate/api-reference/migrate-from-10-to-11/#configureservices) | N/A |
| DolittleClientBuilderCallback | Calllback for building on the [Dolittle client builder](https://dolittle.io/docs/tutorials/getting_started/#connect-the-client-and-commit-an-event) | N/A |
| MongoClientSettingsCallback | Callback for configuring the [MongoDB Driver settings](https://mongodb.github.io/mongo-csharp-driver/2.7/apidocs/html/T_MongoDB_Driver_MongoClientSettings.htm) | N/A |
| PublishAllPublicEvents | Whether or not to add a [filter](https://dolittle.io/docs/concepts/event_handlers_and_filters/#filters) for publishing all events marked as public | True |
| ExposeEventsInGraphQLSchema | Whether or not to add a `events` field with all events as mutations in the GraphQL Schema (development only) | True |

### .MapGraphQL()

The GraphQL endpoint is added when using the `.MapGraphQL()`. In addition to this, in *development* it will also add
the GraphQL playground, available at the same URL as the GraphQL endpoint with a `/ui` appended (e.g. `/graphql/ui`).
The `.MapGraphQL()` is an extension method for `IEndpointRouteBuilder`.

## .AddVanirWithCommon() / .UseVanirWithCommon()

If your needs does not require a lot of flexibility and you're good with Vanir making the choices
for you; you can use the common setup.

The simplest thing that will get you going with the common setup is as follows:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddVanirWithCommon();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseVanirWithCommon();
    }
}
```

The `Add/Use-VanirWithCommon()` methods both call into the `Add/Use-Vanir()` methods, but in addition
to this sets up the following with defaults:

- Swagger
- Controllers
- Static Files

> Note: The `.AddVanirWithCommon()` also accepts taking the `BackendArguments` as described earlier.

## More flexibility

Looking at what the different extension methods do, you'll see that all they do is compose together
default combinations of things through delegating to other implementations within Vanir.
That means that if you want to, you can take full control over the startup process.

The following extension methods for `IServiceCollection` are available:

| Method  | Description |
| -------- | ----------- |
| .AddVanirConfiguration() | Reads the `vanir.json` file and adds its configuration as a configuration object |
| .AddDolittle() | Builds the Dolittle client based on the configuration |
| .AddGraphQL() | Builds the GraphQL Hot Chocolate schema |
| .AddMongoDB() | Adds the MongoDB default settings and serializers |
| .AddResources() | Adds resources that are according to the current execution context, relies on the execution context being configured properly, which .AddDolittle() includes for instance. |
| .AddExecutionContext() | Registers the `IExecutionContextManager` and prepares for execution context to work |

The following extension methods for `IApplicationBuilder` are available:

| Method  | Description |
| -------- | ----------- |
| .UseDolittle() | Starts the Dolittle client |
| .UseGraphQL() | Configures GraphQL endpoints |

| Method | Description |
| ------ | ----------- |
| Blah   | Blah        |

