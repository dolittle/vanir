# Authorization

Authorization is done by leveraging the [ASP.NET `[Authorize]`](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-5.0)
attribute. [Hot Chocolate GraphQL](https://chillicream.com/docs/hotchocolate/v10/server/#supported-core-components) is automatically hooked up for this.

## Configuration

With the ASP.NET Core policy system, you simply configure authorization and adds your policies:

```csharp

public class Startup
{
    public void ConfigureService(IServiceCollection services)
    {
        services.AddAuthorization(options => options
            .AddPolicy("MyPolicy", policy => policy.RequireClaim("Admin")));

        services.AddVanir();
    }
}
```

## Mutations / Queries

Once you have it configured, you can simply start leveraging the `[Authorize]` attribute and reference the required
policy or policies. This can be done either on a class level and then automatically apply to all mutations or queries within
the class, or you can set it specifically for the methods.

> Note: If you add an authorization policy on a class level, any method policies will be in addition to the class level one.

Class level:

```csharp
using Microsoft.AspNetCore.Authorization;
using Dolittle.Vanir.Backend.GraphQL;

[Authorize(Policy = "MyPolicy")]
public class MyMutations : GraphController
{
    [Mutation]
    public async Task<bool> DoSomething()
    {

    }
}
```

Method level:

```csharp
using Microsoft.AspNetCore.Authorization;
using Dolittle.Vanir.Backend.GraphQL;

public class MyMutations : GraphController
{
    [Mutation]
    [Authorize(Policy = "MyPolicy")]
    public async Task<bool> DoSomething()
    {

    }
}
```
