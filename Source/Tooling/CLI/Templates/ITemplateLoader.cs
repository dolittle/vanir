// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.CLI.Templates
{
    public interface ITemplateLoader
    {
        bool Exists(string templateName);
        string Load(string templateName);
    }
}
