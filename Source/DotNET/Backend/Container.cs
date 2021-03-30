// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend
{
    public class Container : Backend.IContainer
    {
        internal static IServiceProvider    ServiceProvider;

        public T Get<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }

        public object Get(Type type)
        {
            return ServiceProvider.GetService(type);
        }
    }
}
