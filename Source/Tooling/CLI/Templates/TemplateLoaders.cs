// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Dolittle.Vanir.CLI.Templates
{
    [Singleton]
    public class TemplateLoaders : ITemplateLoaders
    {
        readonly IEnumerable<ITemplateLoader> _loaders;

        public TemplateLoaders(IEnumerable<ITemplateLoader> loaders)
        {
            _loaders = loaders;
        }

        public string Load(string templateName)
        {
            var loader = _loaders.FirstOrDefault(_ => _.Exists(templateName));
            ThrowIfNoTemplateWasFound(templateName, loader);
            return loader.Load(templateName);
        }

        void ThrowIfNoTemplateWasFound(string templateName, ITemplateLoader loader)
        {
            if (loader == default) throw new TemplateNotFound(templateName);
        }
    }
}
