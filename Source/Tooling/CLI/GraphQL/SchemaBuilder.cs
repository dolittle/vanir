// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dolittle.Vanir.Backend.GraphQL;
using Dolittle.Vanir.Backend.Reflection;
using Dolittle.Vanir.CLI.Reflection;
using TypeInfo = System.Reflection.TypeInfo;
using Types = Dolittle.Vanir.CLI.Reflection.Types;

namespace Dolittle.Vanir.CLI.GraphQL
{
    /// <summary>
    /// Represents an implementation of <see cref="ISchemaBuilder"/>
    /// </summary>
    public class SchemaBuilder : ISchemaBuilder
    {
        static class PrimitiveTypes<T>
        {
            public static readonly TypeDefinition TypeDefinition = new() { Name = typeof(T).Name, Namespace = "", FilePathForImports = "", Type = typeof(T) };
        }

        public static string RootPath = string.Empty;

        /// <inheritdoc/>
        public Schema BuildFrom(Types types)
        {
            var graphControllers = types.Where(_ => _.Implements(typeof(GraphController)));
            var referencedTypes = new List<TypeDefinition>();

            var queries = GetOperationDefinitionsFor<QueryAttribute>(graphControllers, referencedTypes).ToArray();
            var mutations = GetOperationDefinitionsFor<MutationAttribute>(graphControllers, referencedTypes).ToArray();

            return new()
            {
                Mutations = GetOperationDefinitionsFor<MutationAttribute>(graphControllers, referencedTypes).ToArray(),
                Queries = GetOperationDefinitionsFor<QueryAttribute>(graphControllers, referencedTypes).ToArray()
                referencedTypes = referencedTypes.ToArray()
            };
        }

        TypeDefinition GetPrimitiveTypeDefinitionFor(Type type) => typeof(PrimitiveTypes<>).MakeGenericType(type).GetField("TypeDefinition").GetValue(null) as TypeDefinition;

        TypeDefinition CreateAndTypeDefinition(Type type, List<TypeDefinition> typeDefinitions, string filePathForImports = null)
        {
            if (!type.IsComplexType()) return GetPrimitiveTypeDefinitionFor(type);

            if (typeDefinitions.Any(_ => _.Type == type)) return typeDefinitions.Single(_ => _.Type == type);

            if (filePathForImports == null)
            {
                filePathForImports = GetFilePathFor(type);
            }

            var definition = new TypeDefinition
            {
                Name = type.Name,
                Namespace = type.Namespace,
                FilePathForImports = filePathForImports,
                Type = type,
            };

            typeDefinitions.Add(definition);

            definition.Properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(_ =>
                {
                    var type = _.PropertyType;
                    var enumerable = _.PropertyType.IsEnumerable();
                    if (enumerable)
                    {
                        type = type.GetEnumerableElementType();
                    }
                    if (type.IsNullable())
                    {
                        type = type.GetNullableType();
                    }
                    var complex = type.IsComplexType();
                    TypeDefinition typeDefinition = null;
                    if (complex)
                    {
                        CreateAndTypeDefinition(type, typeDefinitions);
                        typeDefinition = typeDefinitions.Single(_ => _.Type == type);
                    }
                    return new PropertyDefinition
                    {
                        Type = type,
                        Name = _.Name,
                        IsEnumerable = enumerable,
                        IsComplex = complex,
                        TypeDefinition = typeDefinition
                    };
                }).ToArray();

            return definition;
        }


        IEnumerable<OperationDefinition> GetOperationDefinitionsFor<TAttribute>(IEnumerable<TypeInfo> graphControllers, List<TypeDefinition> referencedTypes)
            where TAttribute : Attribute, ICanHavePath
        {
            var operationMethods = graphControllers.SelectMany(_ => _.GetMethodsWithAttribute<TAttribute>());
            return operationMethods.Select(_ =>
            {
                var parameters = _.GetParameters();
                var returnType = _.ReturnType;
                if (returnType.IsGenericType)
                {
                    returnType = returnType.GetGenericArguments()[0];
                }
                var typeDefinition = CreateAndTypeDefinition(returnType, referencedTypes);

                return new OperationDefinition
                {
                    Name = _.Name,
                    Namespace = _.DeclaringType.Namespace,
                    FilePathForImports = GetFilePathFor(_.DeclaringType),
                    Method = _,
                    GraphPath = GetGraphRootPath(_),
                    ParameterTypes = parameters.Select(p => CreateAndTypeDefinition(p.ParameterType, referencedTypes)).ToArray(),
                    Parameters = parameters,
                    ReturnType = typeDefinition,
                    IsReturnTypeEnumerable = _.ReturnType.IsEnumerable()
                };
            });
        }

        string[] GetGraphRootPath(MethodInfo method)
        {
            var graphRoot = string.Empty;
            if (method.DeclaringType.HasAttribute<GraphRootAttribute>())
            {
                graphRoot = method.DeclaringType.GetCustomAttribute<GraphRootAttribute>().Path;
            }
            var methodGraphRoot = method.GetCustomAttribute<GraphRootAttribute>();
            if (methodGraphRoot != default)
            {
                if (graphRoot.Length > 0) graphRoot = $"{graphRoot}/";
                graphRoot = $"{graphRoot}{methodGraphRoot.Path}";
            }
            return graphRoot.Split('/');
        }

        string GetFilePathFor(Type type)
        {
            var relative = string.Join('/', type.Namespace.Split(".").Skip(1).ToArray());
            if (relative.Length == 0) return $"{RootPath}/{type.Name}";
            return $"{RootPath}/{relative}/{type.Name}";
        }
    }
}
