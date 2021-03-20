// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using Newtonsoft.Json;

namespace Dolittle.Vanir.Backend.Config
{
    public static class MicroserviceManager
    {
        const string FILENAME = "microservice.json";

        static MicroserviceManager()
        {
            var possibleFiles = new string[] {
                Path.Join(".", FILENAME),
                Path.Join("..", FILENAME)
            };

            foreach(var file in possibleFiles)
            {
                if (File.Exists(file))
                {
                    var json = File.ReadAllText(file);
                    Current = JsonConvert.DeserializeObject<Microservice>(json);
                    break;
                }
            }

            if( Current == null )
            {
                Current = new Microservice();
            }
        }

        public static Microservice Current { get; }
    }
}
