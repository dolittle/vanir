// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;
using static Moq.Times;

namespace Dolittle.Vanir.Backend.GraphQL.for_SchemaRoute.when_configuring
{
    public class and_there_are_no_children_and_no_items : given.an_empty_schema_route
    {
        Because of = () => Configure();

        It should_set_name_on_descriptor = () => object_type_descriptor.Verify(_ => _.Name(type_name), Once());
        It should_add_a_default_field = () => object_type_descriptor.Verify(_ => _.Field("Default"), Once());
    }
}
