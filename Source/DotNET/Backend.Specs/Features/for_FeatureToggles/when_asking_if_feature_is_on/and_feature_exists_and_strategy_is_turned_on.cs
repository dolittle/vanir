// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Reactive.Subjects;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Dolittle.Vanir.Backend.Features.for_FeatureToggles.when_asking_if_feature_is_on
{
    public class and_feature_exists_and_strategy_is_turned_on
    {
        const string Feature = "SomeFeature";
        static FeatureToggles toggles;
        static bool result;

        Establish context = () =>
        {
            var provider = new Mock<IFeaturesProvider>();
            var strategy = new Mock<IFeatureToggleStrategy>();
            strategy.SetupGet(_ => _.IsOn).Returns(true);
            provider.SetupGet(_ => _.Features).Returns(new BehaviorSubject<Features>(new Features(new Dictionary<string, IFeatureToggleStrategy>()
            {
                { Feature, strategy.Object }
            })));
            toggles = new FeatureToggles(provider.Object);
        };

        Because of = () => result = toggles.IsOn(Feature);

        It should_returns_true = () => result.ShouldBeTrue();
    }
}
