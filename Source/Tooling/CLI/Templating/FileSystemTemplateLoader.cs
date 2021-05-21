// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.CLI.Templating
{
    public class FileSystemTemplateLoader : ITemplateLoader
    {
        public bool Exists(string templateName)
        {
            return false;
        }

        public string Load(string templateName)
        {
            return string.Empty;
        }
    }
}
