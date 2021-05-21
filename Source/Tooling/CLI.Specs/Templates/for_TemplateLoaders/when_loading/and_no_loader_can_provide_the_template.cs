// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace Dolittle.Vanir.CLI.Templates.for_TemplateLoaders.when_loading
{
    public class and_no_loader_can_provide_the_template : given.two_template_loaders
    {
        const string template = "my.template.hbs";

        static Exception result;

        Establish context = () =>
        {
            first_loader.Setup(_ => _.Exists(template)).Returns(false);
            second_loader.Setup(_ => _.Exists(template)).Returns(false);
        };

        Because of = () => result = Catch.Exception(() => loaders.Load(template));

        It should_throw_template_not_found = () => result.ShouldBeOfExactType<TemplateNotFound>();
    }
}
