// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using Dolittle.Vanir.CLI.GraphQL;
using Dolittle.Vanir.CLI.Reflection;

namespace Dolittle.Vanir.CLI.ProxyGenerator
{
    public class Generator : IGenerator
    {
        readonly Templates _templates;
        readonly ITypeDiscoverer _typeDiscoverer;
        readonly ISchemaBuilder _schemaBuilder;

        public Generator(Templates templates, ITypeDiscoverer typeDiscoverer, ISchemaBuilder schemaBuilder)
        {
            _templates = templates;
            _typeDiscoverer = typeDiscoverer;
            _schemaBuilder = schemaBuilder;
        }

        public void Generate(FileInfo assembly, DirectoryInfo outputPath)
        {
            var allTypes = _typeDiscoverer.GetAllTypesFromProjectReferencedAssemblies(assembly.FullName);
            var schema = _schemaBuilder.BuildFrom(allTypes);
        }

        /*
        public static void Generate()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                var commandTemplate = Handlebars.Compile(File.ReadAllText("./Templates/CommandTemplate.ts.hbs"));
                var readModelTemplate = Handlebars.Compile(File.ReadAllText("./Templates/ReadModelTemplate.ts.hbs"));
                var queryTemplate = Handlebars.Compile(File.ReadAllText("./Templates/QueryTemplate.ts.hbs"));

                Handlebars.RegisterTemplate("property", File.ReadAllText("./Templates/PropertyTemplate.ts.hbs"));

                var baseOutputPath = Path.Combine(currentDirectory, "../Web/Features");
                TypesExtensions.RootPath = "GeneratedProxies";

                var path = Path.GetFullPath("../Core/bin/Debug/net5.0/Core.dll");
                Console.WriteLine($"Generating from '{path}'");

                var directory = Path.GetDirectoryName(path);
                Directory.SetCurrentDirectory(directory);

                var allTypes = Types.GetAllTypesFromProjectReferencedAssemblies(path, directory);
                var graphControllers = allTypes.Where(_ => _.Implements(typeof(GraphController)));

                var commandTypes = graphControllers.GetCommandTypes();
                var readModelTypes = allTypes.GetReadModelTypes(graphControllers);
                var queryTypes = graphControllers.GetQueryTypes(readModelTypes);

                RenderCommandTypes(commandTemplate, baseOutputPath, commandTypes);
                RenderReadModelTypes(readModelTemplate, baseOutputPath, readModelTypes);
                RenderQueryTypes(queryTemplate, baseOutputPath, queryTypes);
            }
            finally
            {
                Directory.SetCurrentDirectory(currentDirectory);
            }
        }

        static IEnumerable<object> GetPropertiesForReadModel(ReadModelDefinition readModel)
        {
            return readModel.Properties.Select(_ =>
            {
                object readModelProperties = null;
                if (_.IsComplex)
                {
                    readModelProperties = GetPropertiesForReadModel(_.ReadModel);
                }

                return new
                {
                    type = _.Type.ToClientTypeString(),
                    name = _.Name.ToCamelCase(),
                    isComplex = _.IsComplex,
                    properties = readModelProperties
                };
            });
        }

        static void RenderQueryTypes(HandlebarsTemplate<object, object> queryTemplate, string baseOutputPath, IEnumerable<QueryDefinition> queryTypes)
        {
            RenderFrom(queryTypes, queryType =>
            {
                var returnType = queryType.Method.ReturnType.IsEnumerable() ?
                                    queryType.Method.ReturnType.GetEnumerableElementType() :
                                    queryType.Method.ReturnType;

                var actualName = queryType.Name.ToCamelCase();

                return new
                {
                    name = queryType.Name,
                    path = queryType.GraphPath,
                    fullPath = $"{string.Join('.', queryType.GraphPath)}.{actualName}",
                    enumerable = queryType.Enumerable,
                    readModel = queryType.ReadModel.Name,
                    readModelPath = queryType.ReadModel.FilePathForImports,
                    readModelProperties = GetPropertiesForReadModel(queryType.ReadModel),
                    actualName = actualName,
                    parameters = queryType.Method.GetParameters().Select(p => new
                    {
                        name = p.Name.ToCamelCase(),
                        type = p.ParameterType.ToClientTypeString()
                    }),
                    hasParameters = queryType.Method.GetParameters().Length > 0,
                };
            }, baseOutputPath, queryTemplate);
        }

        static void RenderReadModelTypes(HandlebarsTemplate<object, object> readModelTemplate, string baseOutputPath, IEnumerable<ReadModelDefinition> readModelTypes)
        {
            RenderFrom(readModelTypes, readModelType => new
            {
                name = readModelType.Name,

                readModelTypes = readModelType
                                .Properties
                                    .Where(_ => _.IsComplex)
                                    .Select(_ =>
                                    {
                                        var readModelType = readModelTypes.Single(rmt => rmt.Type == _.Type);
                                        return new
                                        {
                                            readModel = readModelType.Name,
                                            readModelPath = readModelType.FilePathForImports
                                        };
                                    }).Distinct(),

                properties = readModelType.Properties.Select(_ =>
                {
                    var type = _.Type.ToClientTypeString();
                    if (_.IsComplex)
                    {
                        var readModelType = readModelTypes.Single(rmt => rmt.Type == _.Type);
                        type = readModelType.Name;
                    }

                    return new
                    {
                        type = type,
                        name = _.Name.ToCamelCase(),
                        enumerable = _.IsEnumerable
                    };
                })
            }, baseOutputPath, readModelTemplate);
        }

        static void RenderCommandTypes(HandlebarsTemplate<object, object> commandTemplate, string baseOutputPath, IEnumerable<CommandDefinition> commandTypes)
        {
            RenderFrom(commandTypes, commandType => new
            {
                name = commandType.Name,
                path = commandType.GraphPath,
                actualName = commandType.Name.ToCamelCase(),
                parameterName = commandType.Parameter.Name,
                properties = commandType.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(_ =>
                {
                    var clientType = _.PropertyType.ToClientTypeString();
                    return new
                    {
                        type = clientType,
                        includeQuotes = clientType == "string",
                        name = _.Name.ToCamelCase()
                    };
                })

            }, baseOutputPath, commandTemplate);
        }

        static void RenderFrom<T>(IEnumerable<T> input, Func<T, object> getTemplateData, string baseOutputPath, HandlebarsTemplate<object, object> template)
            where T : IProxyType
        {
            foreach (var item in input)
            {
                var path = item.FilePathForImports.Replace('/', Path.DirectorySeparatorChar);
                var relativeDirectory = Path.GetDirectoryName(path);
                var commandPath = Path.Combine(baseOutputPath, relativeDirectory, $"{item.Name}.ts");
                var directory = Path.GetDirectoryName(commandPath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var data = getTemplateData(item);
                var result = template(data);

                File.WriteAllText(commandPath, result);
            }
        }
        */
    }
}
