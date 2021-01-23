// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.Config
{
    public class DatabaseConfiguration
    {
        public string Host { get; set; } = "";
        public string Name { get; set; } = "";
        public int Port { get; set; } = 27017;
    }
}
