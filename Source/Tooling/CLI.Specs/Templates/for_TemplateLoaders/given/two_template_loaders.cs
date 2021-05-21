// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;
using Moq;

namespace Dolittle.Vanir.CLI.Templates.for_TemplateLoaders.given
{
    public class two_template_loaders
    {
        protected static Mock<ITemplateLoader> first_loader;
        protected static Mock<ITemplateLoader> second_loader;
        protected static TemplateLoaders loaders;

        Establish context = () =>
        {
            first_loader = new();
            second_loader = new();
            loaders = new TemplateLoaders(new[] { first_loader.Object, second_loader.Object });
        };
    }
}
