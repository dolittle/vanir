// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine;

namespace Dolittle.Vanir.CLI.Tenants
{
    public class TenantsCommandProvider : ICanProvideCommand
    {
        readonly ListTenants _listTenants;
        readonly Init _init;
        readonly AddTenant _addTenant;
        readonly RemoveTenant _removeTenant;

        public TenantsCommandProvider(
            ListTenants listTenants,
            Init init,
            AddTenant addTenant,
            RemoveTenant removeTenant)
        {
            _listTenants = listTenants;
            _init = init;
            _addTenant = addTenant;
            _removeTenant = removeTenant;
        }

        public Command Provide()
        {
            var command = new Command("tenants", "Work with tenants");

            var listCommand = new Command("list", "list all tenants")
            {
                Handler = _listTenants
            };
            command.Add(listCommand);

            var initCommand = new Command("init", "Initializes tenant configuration with defaults for development")
            {
                Handler = _init
            };
            command.Add(initCommand);

            var addCommand = new Command("add", "All a tenant")
            {
                new Argument<Guid>(AddTenant.Id, "Unique identifier for the tenant")
            };
            addCommand.Handler = _addTenant;
            command.Add(addCommand);

            var removeCommand = new Command("remove", "Remove a tenant")
            {
                new Argument<Guid>(RemoveTenant.Id, "Unique identifier for the tenant"),
            };
            removeCommand.Handler = _removeTenant;
            command.Add(removeCommand);

            return command;
        }
    }
}
