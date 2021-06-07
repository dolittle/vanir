// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.MongoDB;
using MongoDB.Bson.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoDBServiceCollectionExtensions
    {
        public static void AddMongoDB(this IServiceCollection _)
        {
            BsonSerializer
                .RegisterSerializationProvider(
                    new ConceptSerializationProvider()
                );
        }
    }
}
