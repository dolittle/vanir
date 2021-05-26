// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Autofac;

namespace Dolittle.Vanir.CLI
{
    public class TypesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(Program).Assembly;
            builder.Register(_ => assembly.DefinedTypes).As<IEnumerable<Type>>();
        }
    }
}
