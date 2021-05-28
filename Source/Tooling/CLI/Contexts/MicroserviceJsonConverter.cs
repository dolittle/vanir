// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Newtonsoft.Json;

namespace Dolittle.Vanir.CLI.Contexts
{
    public class MicroserviceJsonConverter : JsonConverter
    {
        readonly ApplicationContext _applicationContext;
        readonly Func<ApplicationContext, string, MicroserviceContext> _getMicroserviceContextForDirectory;

        public MicroserviceJsonConverter(ApplicationContext applicationContext, Func<ApplicationContext, string , MicroserviceContext> getMicroserviceContextForDirectory)
        {
            _applicationContext = applicationContext;
            _getMicroserviceContextForDirectory = getMicroserviceContextForDirectory;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MicroserviceContext);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var microservicePath = reader.Value.ToString();
            var fullPath = Path.Combine(_applicationContext.Root, microservicePath);
            return _getMicroserviceContextForDirectory(_applicationContext, fullPath);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
