# CLI

Vanir has a CLI tool that covers

## Install

If you want the Vanir CLI to be available globally, install it using:

```shell
$ dotnet tool install -g vanir
```

If you just want it to be local to your project, you can do the following within
the folder of your `.csproj` file:

```shell
$ dotnet tool install vanir
```

## Command groups

The CLI tool consist of different command groups, these are as follows:

| Group | Description |
| ----- | ----------- |
| [Features](./feature-toggling/cli/index.md) | Everything related to feature toggling |

## Suggest / Autocompletion

To enable tab completion for commands of Vanir, you'll have to install [dotnet-suggest](https://github.com/dotnet/command-line-api/blob/main/docs/dotnet-suggest.md).
