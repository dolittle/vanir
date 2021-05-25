// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Linq;
using Dolittle.Vanir.Backend.Features;

namespace Dolittle.Vanir.CLI.Features
{
    public class IsOnColumn : ITableViewColumn<Feature>
    {
        public ColumnDefinition ColumnDefinition => ColumnDefinition.SizeToContent();

        public View Header => new ContentView("IS ON".Underline());

        public View GetCell(Feature item, TextSpanFormatter formatter)
        {
            var isOn = item.Toggles.Any(_ => _.IsOn);
            return new ContentView(isOn ? "True" : "False");
        }
    }
}
