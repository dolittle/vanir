// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.ProxyGenerator
{
    public class TemplateNotFound : ArgumentException
    {
        public TemplateNotFound(string templateName) : base($"Template '{templateName}' was not found")
        {
        }
    }
}
