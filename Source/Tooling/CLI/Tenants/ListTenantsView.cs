// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Rendering.Views;
using Dolittle.Vanir.CLI.Contexts;

namespace Dolittle.Vanir.CLI.Tenants
{
    public class ListTenantsView : StackLayoutView
    {
        public ListTenantsView(ApplicationContext applicationContext)
        {
            Add(new ContentView(""));
            Add(new ContentView("TENANTS FOR".Underline()));
            Add(new ContentView($"Application : {applicationContext.Application.Name} ({applicationContext.Application.Id})"));
            Add(new ContentView(""));

            var table = new TableView<Guid>
            {
                Items = applicationContext.GetTenants()
            };
            table.AddColumn(_ => _, new ContentView("ID".Underline()));
            Add(table);
        }
    }
}
