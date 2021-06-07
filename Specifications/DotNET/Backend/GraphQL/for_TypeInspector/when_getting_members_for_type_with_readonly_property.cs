// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Reflection;
using Dolittle.Vanir.Backend.GraphQL;
using Machine.Specifications;

namespace Dolittle.Vanir.Backend.GraphQL.for_TypeInspector
{
    public class when_getting_members_for_type_with_readonly_property
    {
        class ClassWithReadonlyProperty
        {
            public string WriteProperty { get; set; }
            public string ReadOnlyProperty { get; }
        }

        static TypeInspector inspector;

        static MemberInfo[] members;

        Establish context = () => inspector = new TypeInspector();

        Because of = () => members = inspector.GetMembers(typeof(ClassWithReadonlyProperty)).ToArray();

        It should_only_return_one_member = () => members.Length.ShouldEqual(1);
        It should_only_hold_the_write_property = () => members[0].Name.ShouldEqual("WriteProperty");
    }
}
