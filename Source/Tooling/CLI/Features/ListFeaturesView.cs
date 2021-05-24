// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.CommandLine.Rendering.Views;
using System.Linq;
using Dolittle.Vanir.Backend.Features;

namespace Dolittle.Vanir.CLI.Features
{
    public class ListFeaturesView : StackLayoutView
    {
        public ListFeaturesView(
            ApplicationContext applicationContext,
            MicroserviceContext microserviceContext,
            IEnumerable<Feature> features)
        {
            Add(new ContentView(""));
            Add(new ContentView("FEATURES FOR".Underline()));
            Add(new ContentView($"Application : {applicationContext.Application.Name} ({applicationContext.Application.Id})"));
            Add(new ContentView($"Microservice : {microserviceContext.Microservice.Name} ({microserviceContext.Microservice.Id})"));
            Add(new ContentView(""));

            var table = new TableView<Feature>
            {
                Items = features.ToArray()
            };
            table.AddColumn(_ => _.Name, new ContentView("NAME".Underline()));
            table.AddColumn(_ => _.Description, new ContentView("DESCRIPTION".Underline()));
            table.AddColumn(new IsOnColumn());
            Add(table);
        }
    }
}
