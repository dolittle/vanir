// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Dolittle.Vanir.Backend.Features.for_FeaturesExtensions
{
    public class when_converting_to_json
    {
        const string first_feature = "my.first.feature";
        const string second_feature = "my.second.feature";
        const string first_feature_description = "My first feature";
        const string second_feature_description = "My second feature";

        static Features features;
        static Dictionary<string, FeatureDefinition> result;

        Establish context = () =>
        {
            features = new Features(new Dictionary<string, Feature>
            {
                {
                    first_feature, new Feature
                    {
                        Name = first_feature,
                        Description = first_feature_description,
                        Toggles = new[]
                        {
                            new BooleanFeatureToggle { IsOn = false }
                        }
                    }
                },

                {
                    second_feature, new Feature
                    {
                        Name = second_feature,
                        Description = second_feature_description,
                        Toggles = new[]
                        {
                            new BooleanFeatureToggle { IsOn = true }
                        }
                    }
                }
            });
        };

        Because of = () => result = JsonConvert.DeserializeObject<Dictionary<string, FeatureDefinition>>(features.ToJSON());

        It should_contain_first_feature = () => result.ContainsKey(first_feature).ShouldBeTrue();
        It should_contain_first_features_description = () => result[first_feature].Description.ShouldEqual(first_feature_description);
        It should_have_one_toggle_for_first_feature = () => result[first_feature].Toggles.Length.ShouldEqual(1);
        It should_have_first_features_toggle_set_to_off = () => result[first_feature].Toggles[0].IsOn.ShouldBeFalse();
        It should_not_have_name_on_first_feature = () => result[first_feature].Name.ShouldBeNull();
        It should_contain_second_feature = () => result.ContainsKey(second_feature).ShouldBeTrue();
        It should_contain_second_features_description = () => result[second_feature].Description.ShouldEqual(second_feature_description);
        It should_have_one_toggle_for_second_feature = () => result[second_feature].Toggles.Length.ShouldEqual(1);
        It should_have_second_features_toggle_set_to_on = () => result[second_feature].Toggles[0].IsOn.ShouldBeTrue();
        It should_not_have_name_on_second_feature = () => result[second_feature].Name.ShouldBeNull();
    }
}
