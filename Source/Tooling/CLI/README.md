# CLI

The Vanir CLI is built on top of [Microsofts Command Line API](https://github.com/dotnet/command-line-api) and is also set up with
the [Autofac IoC container](https://autofac.org).

## Writing Commands

Features of the CLI should be put into their own folders / namespaces.
Within a feature, you can also have sub features - which could then reside in a sub folder / namespace of the feature.

For a feature to be visible, it typically exposes commands to the CLI. This is done through
implementing the interface `ICanProvideCommand`.

Example:

```csharp
using System.CommandLine;

namespace Dolittle.Vanir.CLI.MyFeature
{
    public class MyCommand : ICanProvideCommand
    {
        public Command Provide()
        {
            var command = new Command("my-command", "Work with something...")
            {
                new Argument<string>("my-argument", description: "The argument")
            };
            command.Handler = CommandHandler.Create(Handle);
            return command;
        }

        int Handle()
        {
            // Handle the command
        }
    }
}
```

This implementation will then automatically be discovered.

> Note:
> The [Microsofts Command Line API](https://github.com/dotnet/command-line-api) has a lot of overloads for creating command handlers, depending
> on what information you need.

If you want to encapsulate a command and let it be able to take on dependencies of its own without having to have the root feature
command set it all up; you can simply create a class and implement the `ICommandHandler` found in the Microsoft API.

```csharp
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Dolittle.Vanir.CLI.MyFeature
{
    public class MyCommandHandler : ICommandHandler
    {
        public Task<int> InvokeAsync(InvocationContext context)
        {
            return Task.FromResult(0);
        }
    }
}
```

This can then be used in the `MyCommand` type:

```csharp
using System.CommandLine;

namespace Dolittle.Vanir.CLI.MyFeature
{
    public class MyCommand : ICanProvideCommand
    {
        readonly MyCommandHandler _commandHandler;

        public MyCommand(MyCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public Command Provide()
        {
            var command = new Command("my-command", "Work with something...")
            {
                new Argument<string>("my-argument", description: "The argument")
            };
            command.Handler = _commandHandler;
            return command;
        }
    }
}
```

## Contexts

There are some well known contexts that any service/command provider in the CLI can take a dependency to in their constructor.
These contexts are located within the `Contexts` folder - namespace: `Dolittle.Vanir.CLI.Contexts`.
To use these, you will need to get them using the `ContextOf<>` delegate. This is configured in the IoC container and lets
you lazily access the context.

> Note:
> It is better to lazily get the context when needed, as the system configures all types at startup and if they depend on any
> concrete context, it will try to fulfil these at startup and the user of the CLI might be in the wrong context and resolution might fail.

The contexts might be resolved based on a specific file or files at a location. Some of the contexts will try to search for the
closest of these files up the directory hierarchy.

| Type Name | Purpose |
| --------- | ------- |
| `ApplicationContext` | Information about the application the user is performing actions towards |
| `MicroserviceContext` | Information about the microservice the user is performing actions towards |
| `FeaturesContext` | Context for features within a microservice |

### Usage example

Below is an example of using contexts:

```csharp
using System.CommandLine;
using Dolittle.Vanir.CLI.Contexts;

namespace Dolittle.Vanir.CLI.MyFeature
{
    public class MyCommandHandler : ICanProvideCommand
    {
        readonly ContextOf<ApplicationContext> _getApplicationContext;
        readonly ContextOf<MicroserviceContext> getMicroserviceContext;

        public MyCommandHandler(
            ContextOf<ApplicationContext> getApplicationContext,
            ContextOf<MicroserviceContext> getMicroserviceContext)
        {
            _getApplicationContext = getApplicationContext;
            _getMicroserviceContext = getMicroserviceContext;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var applicationContext = _getApplicationContext();
            var microserviceContext = _getMicroserviceContext();

            // Use information from the contexts

            return Task.FromResult(0);
        }
    }
}
```

## Sub-commands

The feature command can define sub commands by creating new commands and add them to the feature command during the `Provide()` method.

```csharp
using System.CommandLine;

namespace Dolittle.Vanir.CLI.MyFeature
{
    public class MyCommand : ICanProvideCommand
    {
        public Command Provide()
        {
            var command = new Command("my-command", "Work with something...")
            {
                new Argument<string>("my-argument", description: "The argument")
            };

            var subCommand = new Command("my-sub-command", "Yellow");
            command.AddCommand(subCommand);
            return command;
        }
    }
}
```

As with the feature command, the command handlers can then either be methods on the class or separate classes.
