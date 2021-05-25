// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dolittle.Vanir.CLI.Templates;

namespace Dolittle.Vanir.CLI.Templating
{
    public class EmbeddedResourceTemplateLoader : ITemplateLoader
    {
        static readonly string _prefix = $"{typeof(DoNotRemove).Namespace}.";
        readonly IEnumerable<string> _resources;

        readonly Assembly _assembly;

        public EmbeddedResourceTemplateLoader()
        {
            _assembly = typeof(Program).Assembly;
            _resources = _assembly.GetManifestResourceNames();
        }

        public bool Exists(string templateName)
        {
            return _resources.Any(_ => _[_prefix.Length..] == templateName);
        }

        public string Load(string templateName)
        {
            var resourceName = _resources.Single(_ => _[_prefix.Length..] == templateName);

            var stream = _assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            return string.Empty;

        }
    }
}
