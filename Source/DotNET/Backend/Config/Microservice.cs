// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend.Config
{
    public class Microservice
    {
        public string Id { get; set; } = Guid.Empty.ToString();
        public string Name { get; set; } = "[Not set]";
        public string Version { get; set; } = "1.0.0";
        public string Commit { get; set; } = "";
        public string Built { get; set; } = "";
    }
}
