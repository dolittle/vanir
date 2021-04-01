// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using HotChocolate.Types;

namespace Dolittle.Vanir.Backend.GraphQL
{
    public class SchemaRoute : ObjectType
    {
        readonly List<SchemaRoute> _children = new();
        readonly List<SchemaRouteItem> _items = new();

        public SchemaRoute(string path, string localName, string typeName)
        {
            Path = path;
            LocalName = localName;
            TypeName = typeName;
        }

        public string Path { get; }
        public string LocalName { get; }
        public string TypeName { get; }

        public void AddChild(SchemaRoute child)
        {
            _children.Add(child);
        }

        public void AddItem(SchemaRouteItem item)
        {
            _items.Add(item);
        }

        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(TypeName);

            foreach (var item in _items)
            {
                descriptor.Field(item.Method).Name(item.Name);
            }

            foreach (var child in _children)
            {
                descriptor.Field(child.LocalName).Type(child).Resolver(_ => new object());
            }

            if (_items.Count == 0)
            {
                descriptor.Field("Default").Resolve(() => "Configure your first item");
            }
        }
    }
}
