// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Autofac;

namespace Dolittle.Vanir.CLI.Templates
{
    public class TemplatesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new ITemplateLoader[] {
                _.Resolve<FileSystemTemplateLoader>(),
                _.Resolve<EmbeddedResourceTemplateLoader>()
            }).As<IEnumerable<ITemplateLoader>>();
        }
    }
}
