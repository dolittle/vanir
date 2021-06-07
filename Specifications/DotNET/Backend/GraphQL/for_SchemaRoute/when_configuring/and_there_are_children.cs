// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.GraphQL;
using Machine.Specifications;
using static Moq.Times;

namespace Dolittle.Vanir.Backend.GraphQL.for_SchemaRoute.when_configuring
{
    public class and_there_are_children : given.an_empty_schema_route
    {
        static SchemaRoute first_child;
        static SchemaRoute second_child;

        Establish context = () =>
        {
            first_child = new SchemaRoute("FirstChildPath", "FirstChildLocalName", "FirstChildTypeName");
            second_child = new SchemaRoute("SecondChildPath", "SecondChildLocalName", "SecondChildTypeName");
            route.AddChild(first_child);
            route.AddChild(second_child);
        };
        Because of = () => Configure();

        It should_add_a_field_for_first_child = () => object_type_descriptor.Verify(_ => _.Field(first_child.LocalName), Once());
        It should_add_a_field_for_second_child = () => object_type_descriptor.Verify(_ => _.Field(second_child.LocalName), Once());
        It should_set_type_for_first_child = () => object_field_descriptor.Verify(_ => _.Type(first_child), Once());
        It should_set_type_for_second_child = () => object_field_descriptor.Verify(_ => _.Type(second_child), Once());
    }
}
