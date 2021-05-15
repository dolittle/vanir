// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Dolittle.Vanir.Backend.Features.for_FeatureToggles.when_asking_if_feature_is_on
{
    public class and_feature_does_not_exist
    {
        static FeatureToggles toggles;
        static bool result;

        Establish context = () =>
        {
            var provider = new Mock<IFeaturesProvider>();
            provider.Setup(_ => _.Provide()).Returns(new Features());
            toggles = new FeatureToggles(provider.Object);
        };

        Because of = () => result = toggles.IsOn("SomeFeature");

        It should_returns_false = () => result.ShouldBeFalse();
    }
}
