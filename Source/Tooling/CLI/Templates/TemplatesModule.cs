// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;

namespace Dolittle.Vanir.CLI.Templates
{
    public class TemplatesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TemplateLoaders>().As<ITemplateLoaders>().SingleInstance();
            base.Load(builder);
        }
    }
}
