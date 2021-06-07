// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Machine.Specifications;

namespace Dolittle.Vanir.Backend.Features.for_FeaturesParser
{
    public class when_parsing_with_two_features_with_one_toggle_each
    {
        const string first_feature = "my.first.feature";
        const string second_feature = "my.second.feature";

        const string json =
        "{" +
        "   \"my.first.feature\": {" +
        "       \"description\": \"The first feature\"," +
        "       \"toggles\": [{" +
        "           \"type\": \"Boolean\"," +
        "           \"isOn\": false" +
        "       }]" +
        "   }," +
        "   \"my.second.feature\": {" +
        "       \"description\": \"The second feature\"," +
        "       \"toggles\": [{" +
        "           \"type\": \"Boolean\"," +
        "           \"isOn\": true" +
        "       }]" +
        "   }" +
        "}";

        static FeaturesParser parser;
        static Features result;

        Establish context = () => parser = new();

        Because of = () => result = parser.Parse(json);

        It should_contain_first_feature = () => result.ContainsKey(first_feature).ShouldBeTrue();
        It should_hold_first_features_description = () => result[first_feature].Description.ShouldEqual("The first feature");
        It should_have_one_toggle_for_first_feature = () => result[first_feature].Toggles.Count().ShouldEqual(1);
        It should_have_toggle_for_first_feature_set_to_false = () => result[first_feature].Toggles.First().IsOn.ShouldBeFalse();
        It should_contain_second_feature = () => result.ContainsKey(second_feature).ShouldBeTrue();
        It should_hold_second_features_description = () => result[second_feature].Description.ShouldEqual("The second feature");
        It should_have_one_toggle_for_second_feature = () => result[second_feature].Toggles.Count().ShouldEqual(1);
        It should_have_toggle_for_second_feature_set_to_true = () => result[second_feature].Toggles.First().IsOn.ShouldBeTrue();
    }
}
