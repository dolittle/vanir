// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend
{
    public interface IContainer
    {
        T Get<T>();

        object Get(Type type);
    }
}
