// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Dolittle.Vanir.CLI.GraphQL
{
    public class QueryDefinition : ISchemaType
    {
        public string Name { get; init; }
        public string Namespace { get; init; }
        public string FilePathForImports { get; init; }
        public MethodInfo Method { get; init; }
        public string[] GraphPath { get; init; }
        public ReadModelDefinition ReadModel { get; init; }
        public bool Enumerable { get; init; }
    }
}
