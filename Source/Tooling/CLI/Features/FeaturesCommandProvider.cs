// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;

namespace Dolittle.Vanir.CLI.Features
{
    public class FeaturesCommandProvider : ICanProvideCommand
    {
        readonly ListFeatures _listFeatures;
        readonly AddFeature _addFeature;
        readonly RemoveFeature _removeFeature;
        readonly SwitchOnFeature _switchOnFeature;
        readonly SwitchOffFeature _switchOffFeature;

        public FeaturesCommandProvider(
            ListFeatures listFeatures,
            AddFeature addFeature,
            RemoveFeature removeFeature,
            SwitchOnFeature switchOnFeature,
            SwitchOffFeature switchOffFeature)
        {
            _listFeatures = listFeatures;
            _addFeature = addFeature;
            _removeFeature = removeFeature;
            _switchOnFeature = switchOnFeature;
            _switchOffFeature = switchOffFeature;
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
                new Argument<string>(RemoveFeature.NameArgument, description: "Name of the feature")
            };
            removeCommand.Handler = _removeFeature;
            command.AddCommand(removeCommand);

            var switchOnCommand = new Command("switch-on", "Switch on a feature toggle")
            {
                new Argument<string>(SwitchOnFeature.NameArgument, description: "Name of the feature to switch on")
            };
            switchOnCommand.Handler = _swi;
            command.AddCommand(switchOnCommand);

            var switchOffCommand = new Command("switch-off", "Switch on a feature toggle")
            {
                new Argument<string>(SwitchOffFeature.NameArgument, description: "Name of the feature to switch off")
            };
            switchOffCommand.Handler = _switchOffFeature;
            command.AddCommand(switchOffCommand);

            return command;
        }
    }
}
