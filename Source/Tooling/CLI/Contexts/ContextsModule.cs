// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using Autofac;
using Dolittle.Vanir.Backend.Config;
using Dolittle.Vanir.Backend.Features;
using Newtonsoft.Json;

namespace Dolittle.Vanir.CLI.Contexts
{

    public class ContextsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ContextOf<ApplicationContext>>(_ => () => ProvideApplicationContext()).As<ContextOf<ApplicationContext>>();
            builder.Register<ContextOf<MicroserviceContext>>(_ => () => ProvideMicroserviceContext()).As<ContextOf<MicroserviceContext>>();
            builder.Register<ContextOf<FeaturesContext>>(_ => () => ProvideFeatures()).As<ContextOf<FeaturesContext>>();
            base.Load(builder);
        }

        ApplicationContext ProvideApplicationContext()
        {
            var file = SearchForFile(Directory.GetCurrentDirectory(), "application.json", out bool found);
            if (!found)
            {
                Console.Error.WriteLine("`application.json` was not found in the directory or in any parent directories.");
                Environment.Exit(-1);
                return null;
            }

            var json = File.ReadAllText(file);
            var context = new ApplicationContext
            {
                File = file,
                Root = Path.GetDirectoryName(file),
                DolittleFolder = Path.Combine(Path.GetDirectoryName(file), ".dolittle")
            };
            context.Application = JsonConvert.DeserializeObject<Application>(json, new MicroserviceJsonConverter(context, ProvideMicroserviceContext));

            return context;
        }

        MicroserviceContext ProvideMicroserviceContext(ApplicationContext applicationContext = null, string directory = null)
        {
            if (directory == null) directory = Directory.GetCurrentDirectory();
            if (applicationContext == null) applicationContext = ProvideApplicationContext();

            var file = SearchForFile(directory, "microservice.json", out bool found);
            if (!found)
            {
                Console.Error.WriteLine("`microservice.json` was not found in the directory or in any parent directories.");
                Environment.Exit(-1);
                return null;
            }

            var root = Path.GetDirectoryName(file);
            var dolittleFolder = Path.Join(directory, ".dolittle");
            if (!Directory.Exists(dolittleFolder))
            {
                dolittleFolder = string.Empty;
            }

            if (string.IsNullOrEmpty(dolittleFolder))
            {
                var dolittleFolders = Directory.GetDirectories(root).Where(_ => Directory.Exists(Path.Join(_, ".dolittle"))).ToArray();
                if (dolittleFolders.Length == 0)
                {
                    Console.Error.WriteLine("Can't locate the '.dolittle' folder. Typically this should be located within your backend project.");
                    Environment.Exit(-1);
                    return null;
                }

                if (dolittleFolders.Length > 1)
                {
                    Console.Error.WriteLine("There are multiple candidates for the `.dolittle` folder. Either make sure you only have one or run the Vanir CLI from the folder that holds the `.dolittle` folder.");
                    Environment.Exit(-1);
                    return null;
                }

                dolittleFolder = Path.Join(dolittleFolders[0], ".dolittle");
            }

            var json = File.ReadAllText(file);
            return new MicroserviceContext
            {
                Application = applicationContext,
                Microservice = JsonConvert.DeserializeObject<Backend.Config.Microservice>(json),
                File = file,
                Root = root,
                DolittleFolder = dolittleFolder
            };
        }

        FeaturesContext ProvideFeatures()
        {
            var microserviceContext = ProvideMicroserviceContext();

            var file = Path.Join(microserviceContext.DolittleFolder, "features.json");
            if (!File.Exists(file))
            {
                return new()
                {
                    Features = new(),
                    File = file
                };
            }

            var json = File.ReadAllText(file);
            return new FeaturesContext
            {
                Features = new FeaturesParser().Parse(json),
                File = file
            };
        }

        string SearchForFile(string directory, string file, out bool found)
        {
            var applicationJsonFile = Path.Join(directory, file);
            if (File.Exists(applicationJsonFile))
            {
                found = true;
                return applicationJsonFile;
            }

            if (Directory.GetDirectoryRoot(directory) != directory)
            {
                var fileFound = SearchForFile(Directory.GetParent(directory).FullName, file, out bool wasFound);
                if (wasFound)
                {
                    found = true;
                    return fileFound;
                }
            }

            found = false;
            return string.Empty;
        }
    }
}
