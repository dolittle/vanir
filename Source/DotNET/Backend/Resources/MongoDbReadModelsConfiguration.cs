// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.Resources
{
    public class MongoDbReadModelsConfiguration : ResourceConfiguration
    {
        public string Host { get; set; }
        public string Database { get; set; }
        public bool UseSSL { get; set; }
    }
}
