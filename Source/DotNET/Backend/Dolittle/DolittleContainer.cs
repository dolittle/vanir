// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.SDK.Execution;
using Dolittle.Vanir.Backend.Execution;

namespace Dolittle.Vanir.Backend.Dolittle
{
    public class DolittleContainer : global::Dolittle.SDK.DependencyInversion.IContainer
    {
        internal static IServiceProvider ServiceProvider;

        public object Get(Type service, ExecutionContext context)
        {
            ExecutionContextManager.SetCurrent(context);
            return ServiceProvider.GetService(service);
        }

        public T Get<T>(ExecutionContext context) where T : class
        {
            ExecutionContextManager.SetCurrent(context);
            return (T)ServiceProvider.GetService(typeof(T));
        }
    }
}
