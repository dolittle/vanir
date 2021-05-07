// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Dolittle.Vanir.ProxyGenerator.Specs.for_TemplateLoaders.when_loading
{
    public class and_the_first_loader_does_not_have_template : given.two_template_loaders
    {
        const string template = "my.template.hbs";
        const string template_content = "This is the content";

        static string result;

        Establish context = () =>
        {
            first_loader.Setup(_ => _.Exists(template)).Returns(false);
            second_loader.Setup(_ => _.Exists(template)).Returns(true);
            second_loader.Setup(_ => _.Load(template)).Returns(template_content);
        };

        Because of = () => result = loaders.Load(template);

        It should_not_load_from_first_loader = () => first_loader.Verify(_ => _.Load(template), Times.Never);
        It should_return_the_expected_content = () => result.ShouldEqual(template_content);
    }
}
