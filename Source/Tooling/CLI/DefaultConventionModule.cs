// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Autofac;
using Dolittle.Vanir.Backend.Reflection;

namespace Dolittle.Vanir.CLI
{
    public class DefaultConventionModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var assembly = typeof(Program).Assembly;
            var conventionBasedTypes = assembly.DefinedTypes.Where(_ =>
            {
                var interfaces = _.GetInterfaces();
                if (interfaces.Length > 0) return interfaces.Any(i => i.Name == $"I{_.Name}");
                return false;
            });

            foreach (var conventionBasedType in conventionBasedTypes)
            {
                var interfaceToBind = conventionBasedType.GetInterfaces().Single(_ => _.Name == $"I{conventionBasedType.Name}");
                var builder = containerBuilder.RegisterType(conventionBasedType).As(interfaceToBind);
                if (conventionBasedType.HasAttribute<SingletonAttribute>()) builder.SingleInstance();
            }
        }
    }
}
