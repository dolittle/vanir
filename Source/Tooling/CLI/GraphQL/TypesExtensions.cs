// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dolittle.Vanir.Backend.GraphQL;
using Dolittle.Vanir.Backend.Reflection;
using Dolittle.Vanir.CLI.Reflection;

namespace Dolittle.Vanir.CLI.GraphQL
{
    public static class TypesExtensions
    {
        public static string RootPath = string.Empty;

        static void CreateAndAddReadModelDefinition(Type type, List<ReadModelDefinition> readModels, string filePathForImports = null)
        {
            if (readModels.Any(_ => _.Type == type)) return;

            if (filePathForImports == null)
            {
                filePathForImports = GetFilePathFor(type);
            }

            var definition = new ReadModelDefinition
            {
                Name = type.Name,
                Namespace = type.Namespace,
                FilePathForImports = filePathForImports,
                Type = type,
                Properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(_ =>
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
                    ReadModelDefinition readModel = null;
                    if (complex)
                    {
                        CreateAndAddReadModelDefinition(type, readModels);
                        readModel = readModels.Single(_ => _.Type == type);
                    }
                    return new PropertyDefinition
                    {
                        Type = type,
                        Name = _.Name,
                        IsEnumerable = enumerable,
                        IsComplex = complex,
                        ReadModel = readModel
                    };
                }).ToArray()
            };

            readModels.Add(definition);
        }

        public static IEnumerable<ReadModelDefinition> GetReadModelTypes(this IEnumerable<Type> allTypes, IEnumerable<TypeInfo> graphControllers)
        {
            var readModelDefinitions = new List<ReadModelDefinition>();

            var queryMethods = graphControllers.GetQueryMethods();
            var queryReadModels = queryMethods.Select(_ => _.ReturnType.IsEnumerable() ?
                                     _.ReturnType.GetEnumerableElementType() :
                                     _.ReturnType);

            var readModelTypes = allTypes
                        .Where(_ => _.Implements(typeof(object)) && !_.IsInterface && _.GetGenericArguments().Length == 0)
                        .ToList();

            readModelTypes.AddRange(queryReadModels.Where(_ => !readModelTypes.Any(r => r == _)));

            readModelTypes.ForEach(_ => CreateAndAddReadModelDefinition(_, readModelDefinitions));
            return readModelDefinitions;
        }

        static IEnumerable<MethodInfo> GetQueryMethods(this IEnumerable<TypeInfo> graphControllers)
        {
            return graphControllers.SelectMany(_ => _.GetMethodsWithAttribute<QueryAttribute>());
        }

        public static IEnumerable<QueryDefinition> GetQueryTypes(this IEnumerable<TypeInfo> graphControllers, IEnumerable<ReadModelDefinition> readModels)
        {
            var queryMethods = graphControllers.GetQueryMethods();
            return queryMethods.Select(_ =>
            {
                var returnType = _.ReturnType.IsEnumerable() ?
                                     _.ReturnType.GetEnumerableElementType() :
                                     _.ReturnType;

                var readModel = readModels.Single(_ => _.Type == returnType);

                return new QueryDefinition
                {
                    Name = _.DeclaringType.Name,
                    Namespace = _.DeclaringType.Namespace,
                    FilePathForImports = GetFilePathFor(_.DeclaringType),
                    GraphPath = GetGraphRootPath(_),
                    Method = _,
                    ReadModel = readModel,
                    Enumerable = _.ReturnType.IsEnumerable()
                };
            });
        }

        public static IEnumerable<MutationDefinition> GetMutationTypes(this IEnumerable<TypeInfo> graphControllers)
        {
            var mutationMethods = graphControllers.SelectMany(_ => _.GetMethodsWithAttribute<MutationAttribute>());
            return mutationMethods.Select(_ =>
            {
                var parameters = _.GetParameters();
                if (parameters.Length == 1)
                {
                    return new MutationDefinition
                    {
                        Name = _.Name,
                        Namespace = parameters[0].ParameterType.Namespace,
                        FilePathForImports = GetFilePathFor(_.DeclaringType),
                        Parameter = parameters[0],
                        Type = parameters[0].ParameterType,
                        GraphPath = GetGraphRootPath(_)
                    };
                }
                return default;
            }).Where(_ => _ != default);
        }

        static string[] GetGraphRootPath(MethodInfo method)
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

        static string GetFilePathFor(Type type)
        {
            var relative = string.Join('/', type.Namespace.Split(".").Skip(1).ToArray());
            if (relative.Length == 0) return $"{RootPath}/{type.Name}";
            return $"{RootPath}/{relative}/{type.Name}";
        }
    }
}
