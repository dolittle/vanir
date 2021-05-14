// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using HandlebarsDotNet;
using Dolittle.Vanir.CLI.Templating;

namespace Dolittle.Vanir.CLI.ProxyGenerator
{
   public class Templates
    {
        readonly ITemplateLoaders _templateLoaders;

        public Templates(ITemplateLoaders templateLoader)
        {
            _templateLoaders = templateLoader;

            Mutation = Handlebars.Compile(_templateLoaders.Load("ProxyGeneration.Mutation.hbs"));
            Query = Handlebars.Compile(_templateLoaders.Load("ProxyGeneration.Query.hbs"));
            Type = Handlebars.Compile(_templateLoaders.Load("ProxyGeneration.Type.hbs"));
            var propertyTemplate = _templateLoaders.Load("ProxyGeneration.Property.hbs");
            Property = Handlebars.Compile(propertyTemplate);
            Handlebars.RegisterTemplate("property", propertyTemplate);
        }

        public HandlebarsTemplate<object, object> Mutation { get; }

        public HandlebarsTemplate<object, object> Query { get; }

        public HandlebarsTemplate<object, object> Type { get; }

        public HandlebarsTemplate<object, object> Property { get; }
    }
}
