// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Vanir.Backend.Dolittle;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class DolittleAppBuilderExtensions
    {
        public static void UseDolittle(this IApplicationBuilder app)
        {
            ThrowIfClientBuilderNotCreated();
            app.UseExecutionContext();
            DolittleContainer.ServiceProvider = app.ApplicationServices;
            var client = DolittleServiceCollectionExtensions.DolittleClientBuilder.Build();
            DolittleServiceCollectionExtensions.DolittleClient = client;
            client.WithContainer(new DolittleContainer()).Start();
        }

        static void ThrowIfClientBuilderNotCreated()
        {
            if (DolittleServiceCollectionExtensions.DolittleClientBuilder == null)
            {
                throw new ArgumentException("You need to make sure AddDolittle() or AddVanir() is called before UseDolittle() or UseVanir()");
            }
        }
    }
}
