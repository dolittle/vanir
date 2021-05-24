// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Autofac;
using Dolittle.Vanir.Backend.Config;
using Dolittle.Vanir.Backend.Features;
using Newtonsoft.Json;

namespace Dolittle.Vanir.CLI
{
    public class ContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ContextOf<Application>>(_ => () => ProvideApplicationContext()).As<ContextOf<Application>>();
            builder.Register<ContextOf<Microservice>>(_ => () => ProvideMicroserviceContext()).As<ContextOf<Microservice>>();
            builder.Register<ContextOf<Backend.Features.Features>>(_ => () => ProvideFeatures()).As<ContextOf<Backend.Features.Features>>();
            base.Load(builder);
        }

        Application ProvideApplicationContext()
        {
            var file = SearchForFile(Directory.GetCurrentDirectory(), "application.json", out bool found);
            if (!found)
            {
                Console.Error.WriteLine("`application.json` was not found in the directory or in any parent directories.");
                Environment.Exit(-1);
                return null;
            }

            var json = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Application>(json);
        }

        Microservice ProvideMicroserviceContext()
        {
            var file = SearchForFile(Directory.GetCurrentDirectory(), "microservice.json", out bool found);
            if (!found)
            {
                Console.Error.WriteLine("`microservice.json` was not found in the directory or in any parent directories.");
                Environment.Exit(-1);
                return null;
            }

            var json = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Microservice>(json);
        }

        Backend.Features.Features ProvideFeatures()
        {
            var file = Path.Join(Directory.GetCurrentDirectory(), "data", "features.json");
            if (!File.Exists(file))
            {
                Console.Error.WriteLine("`features.json` was not found in the data directory of current directory.");
                Environment.Exit(-1);
                return null;
            }

            var json = File.ReadAllText(file);
            return new FeaturesParser().Parse(json);
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
