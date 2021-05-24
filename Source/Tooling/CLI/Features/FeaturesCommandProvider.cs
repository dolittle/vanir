// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dolittle.Vanir.CLI.Features
{
    public class FeaturesCommandProvider : ICanProvideCommand
    {
        readonly ListFeatures _listFeatures;
        readonly AddFeature _addFeature;

        public FeaturesCommandProvider(ListFeatures listFeatures, AddFeature addFeature)
        {
            _listFeatures = listFeatures;
            _addFeature = addFeature;
        }

        public Command Provide()
        {
            var command = new Command("features", "Work with features in the system")
            {
                new Option<string>("--environment", description: "Environment to apply to (defaults to local)")
            };

            var listCommand = new Command("list", "List all features")
            {
                Handler = _listFeatures
            };
            command.AddCommand(listCommand);

            var addCommand = new Command("add", "Add a feature")
            {
                new Argument<string>(AddFeature.NameArgument, description: "Name of the feature"),
                new Argument<string>(AddFeature.DescriptionArgument, getDefaultValue: () => string.Empty, description: "Description for the feature"),
            };
            addCommand.Handler = _addFeature;
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
