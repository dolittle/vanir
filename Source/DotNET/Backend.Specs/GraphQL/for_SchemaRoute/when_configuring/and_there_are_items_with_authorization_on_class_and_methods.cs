// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Dolittle.Vanir.Backend.GraphQL;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Machine.Specifications;
using Moq;
using static Moq.It;
using static Moq.Times;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using It = Machine.Specifications.It;

namespace Dolittle.Vanir.Backend.Specs.GraphQL.for_SchemaRoute.when_configuring
{
    public class and_there_are_items_with_authorization_on_class_and_methods : given.an_empty_schema_route
    {
        [Authorize(Policy = "ClassPolicy")]
        class ClassWithMethods
        {
            [Authorize(Policy = "FirstMethodPolicy")]
            public void FirstMethod() { }

            [Authorize(Policy = "SecondMethodPolicy")]
            public void SecondMethod() { }
        }

        static SchemaRouteItem first_item;
        static SchemaRouteItem second_item;

        static Mock<IObjectFieldDescriptor> first_method_descriptor;
        static Mock<IObjectFieldDescriptor> second_method_descriptor;

        Establish context = () =>
        {
            var methods = typeof(ClassWithMethods).GetMethods(BindingFlags.Instance | BindingFlags.Public);
            first_item = new SchemaRouteItem(methods[0], "FirstItem");
            route.AddItem(first_item);
            second_item = new SchemaRouteItem(methods[1], "SecondItem");
            route.AddItem(second_item);

            first_method_descriptor = new();
            first_method_descriptor.Setup(_ => _.Type(IsAny<SchemaRoute>())).Returns(first_method_descriptor.Object);
            first_method_descriptor.Setup(_ => _.Name(IsAny<NameString>())).Returns(first_method_descriptor.Object);

            second_method_descriptor = new();
            second_method_descriptor.Setup(_ => _.Type(IsAny<SchemaRoute>())).Returns(second_method_descriptor.Object);
            second_method_descriptor.Setup(_ => _.Name(IsAny<NameString>())).Returns(second_method_descriptor.Object);

            object_type_descriptor.Setup(_ => _.Field(first_item.Method)).Returns(first_method_descriptor.Object);
            object_type_descriptor.Setup(_ => _.Field(second_item.Method)).Returns(second_method_descriptor.Object);

            object_type_descriptor.Setup(_ => _.Directive(IsAny<AuthorizeDirective>())).Returns(object_type_descriptor.Object);
        };

        Because of = () => Configure();

        It should_add_field_for_first_method = () => object_type_descriptor.Verify(_ => _.Field(first_item.Method), Once());
        It should_add_field_for_second_method = () => object_type_descriptor.Verify(_ => _.Field(second_item.Method), Once());
        It should_set_name_for_first_method = () => object_field_descriptor.Verify(_ => _.Name(first_item.Name), Once());
        It should_set_name_for_second_method = () => object_field_descriptor.Verify(_ => _.Name(second_item.Name), Once());

        It should_set_method_authorization_for_first_method = () => first_method_descriptor.Verify(_ => _.Directive(Is<AuthorizeDirective>(a => a.Policy == "FirstMethodPolicy")), Once());
    }
}
