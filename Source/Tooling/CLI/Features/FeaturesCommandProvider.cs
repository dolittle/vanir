// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using Dolittle.Vanir.Backend.Config;

namespace Dolittle.Vanir.CLI.Features
{
    public class FeaturesCommandProvider : ICanProvideCommand
    {
        readonly ApplicationContext _applicationContext;
        readonly MicroserviceContext _microserviceContext;
        readonly FeaturesContext _featuresContext;

        public FeaturesCommandProvider(
            ApplicationContext applicationContext,
            MicroserviceContext microserviceContext,
            FeaturesContext featuresContext)
        {
            _applicationContext = applicationContext;
            _microserviceContext = microserviceContext;
            _featuresContext = featuresContext;
        }

        public Command Provide()
        {
            var command = new Command("features", "Work with features in the system")
            {
                new Option<string>("--environment", description: "Environment to apply to (defaults to local)")
            };

            var listCommand = new Command("list", "List all features")
            {
                Handler = new ListFeatures(
                    _applicationContext,
                    _microserviceContext,
                    _featuresContext)
            };
            command.AddCommand(listCommand);

            var addCommand = new Command("add", "Add a feature")
            {
                new Argument<string>("name", description: "Name of the feature"),
                new Option<string>("--description", description: "Description of the feature")
            };
            command.AddCommand(addCommand);

            var removeCommand = new Command("remove", "Remove a feature")
            {
                new Argument<string>("name", description: "Name of the feature")
            };
            command.AddCommand(removeCommand);

            var toggleCommand = new Command("toggle", "Toggle on or off a feature")
            {
                new Argument<string>("feature", description: "Name of the feature to toggle")
            };
            command.AddCommand(toggleCommand);

            return command;
        }
    }
}
