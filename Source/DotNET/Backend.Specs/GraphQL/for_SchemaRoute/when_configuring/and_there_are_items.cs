// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Dolittle.Vanir.Backend.GraphQL;
using Machine.Specifications;
using static Moq.Times;

namespace Dolittle.Vanir.Backend.Specs.GraphQL.for_SchemaRoute.when_configuring
{
    public class and_there_are_items : given.an_empty_schema_route
    {
        class ClassWithMethods
        {
            public void FirstMethod() { }
            public void SecondMethod() { }
        }

        static SchemaRouteItem first_item;
        static SchemaRouteItem second_item;

        Establish context = () =>
        {
            var methods = typeof(ClassWithMethods).GetMethods(BindingFlags.Instance|BindingFlags.Public);
            first_item = new SchemaRouteItem(methods[0], "FirstItem");
            route.AddItem(first_item);
            second_item = new SchemaRouteItem(methods[1], "SecondItem");
            route.AddItem(second_item);
        };

        Because of = () => Configure();

        It should_add_field_for_first_method = () => object_type_descriptor.Verify(_ => _.Field(first_item.Method), Once());
        It should_add_field_for_second_method = () => object_type_descriptor.Verify(_ => _.Field(second_item.Method), Once());
        It should_set_name_for_first_method = () => object_field_descriptor.Verify(_ => _.Name(first_item.Name), Once());
        It should_set_name_for_second_method = () => object_field_descriptor.Verify(_ => _.Name(second_item.Name), Once());
    }
}
