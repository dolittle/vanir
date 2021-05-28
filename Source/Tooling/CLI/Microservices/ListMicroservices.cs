// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Dolittle.Vanir.CLI.Contexts;

namespace Dolittle.Vanir.CLI.Microservices
{
    public class ListMicroservices : ICommandHandler
    {
        readonly ContextOf<ApplicationContext> _getApplicationContext;

        public ListMicroservices(ContextOf<ApplicationContext> getApplicationContext)
        {
            _getApplicationContext = getApplicationContext;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var view = new ListMicroservicesView(_getApplicationContext());
            context.Render(view);
            return Task.FromResult(0);
        }
    }
}
