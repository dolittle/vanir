// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Reactive.Subjects;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Dolittle.Vanir.Backend.Features.for_FeatureToggles.when_asking_if_feature_is_on
{
    public class and_feature_exists_and_toggle_is_turned_on
    {
        const string FeatureName = "SomeFeature";
        static FeatureToggles toggles;
        static bool result;

        Establish context = () =>
        {
            var provider = new Mock<IFeaturesProvider>();
            var toggle = new Mock<IFeatureToggle>();
            toggle.SetupGet(_ => _.IsOn).Returns(true);
            var feature = new Feature
            {
                Name = FeatureName,
                Description = string.Empty,
                Toggles = new[] { toggle.Object }
            };

            provider.SetupGet(_ => _.Features).Returns(new BehaviorSubject<Features>(new Features(new Dictionary<string, Feature>()
            {
                { FeatureName, feature }
            })));
            toggles = new FeatureToggles(provider.Object);
        };

        Because of = () => result = toggles.IsOn(FeatureName);

        It should_returns_true = () => result.ShouldBeTrue();
    }
}
