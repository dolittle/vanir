// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine.Rendering.Views;
using Dolittle.Vanir.CLI.Contexts;

namespace Dolittle.Vanir.CLI.Microservices
{
    public class ListMicroservicesView : StackLayoutView
    {
        public ListMicroservicesView(ApplicationContext applicationContext)
        {
            Add(new ContentView(""));
            Add(new ContentView("MICROSERVICES FOR".Underline()));
            Add(new ContentView($"Application : {applicationContext.Application.Name} ({applicationContext.Application.Id})"));
            Add(new ContentView(""));

            var table = new TableView<MicroserviceContext>
            {
                Items = applicationContext.Application.Microservices
            };
            table.AddColumn(_ => _.Microservice.Id, new ContentView("ID".Underline()));
            table.AddColumn(_ => _.Microservice.Name, new ContentView("NAME".Underline()));
            table.AddColumn(_ => _.Root, new ContentView("PATH".Underline()));
            Add(table);
        }
    }
}
