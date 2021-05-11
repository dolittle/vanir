// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Dolittle.Vanir.Backend.GraphQL;
using HotChocolate;
using HotChocolate.Types;
using Machine.Specifications;
using Moq;
using static Moq.It;

namespace Dolittle.Vanir.Backend.Specs.GraphQL.for_SchemaRoute.given
{
    public class an_empty_schema_route
    {
        protected static string path = "SomePath";
        protected static string local_name = "LocalName";
        protected static string type_name = "TypeName";
        protected static SchemaRoute route;
        protected static Mock<IObjectTypeDescriptor> object_type_descriptor;
        protected static Mock<IObjectFieldDescriptor> object_field_descriptor;

        static MethodInfo configure_method;

        Establish context = () =>
        {
            route = new SchemaRoute(path, local_name, type_name);

            configure_method = typeof(SchemaRoute).GetMethod("Configure", BindingFlags.Instance | BindingFlags.NonPublic);
            object_type_descriptor = new();
            object_field_descriptor = new();
            object_type_descriptor.Setup(_ => _.Field(IsAny<NameString>())).Returns(object_field_descriptor.Object);
            object_type_descriptor.Setup(_ => _.Field(IsAny<MethodInfo>())).Returns(object_field_descriptor.Object);
            object_field_descriptor.Setup(_ => _.Type(IsAny<SchemaRoute>())).Returns(object_field_descriptor.Object);
            object_field_descriptor.Setup(_ => _.Name(IsAny<NameString>())).Returns(object_field_descriptor.Object);
        };

        protected static void Configure(IObjectTypeDescriptor descriptor = default)
        {
            if (descriptor == default) descriptor = object_type_descriptor.Object;
            configure_method.Invoke(route, new[] { descriptor });
        }
    }
}
