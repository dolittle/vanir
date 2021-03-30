// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend.Config
{
    public class Configuration
    {
        public string RouteSegment { get; set; } = "";
        public bool IsRooted { get; set; } = false;
        public int Port { get; set; } = 80;
        public Guid MicroserviceId { get; set; } = Guid.Empty;
        public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();
        public DatabaseConfiguration EventStore { get; set; } = new DatabaseConfiguration();
        public DolittleConfiguration Dolittle { get; set; } = new DolittleConfiguration();
        public string Environment { get; set; } = "development";

        public bool IsDevelopment => Environment == "development";
        public bool IsProduction => Environment == "production";

        public string Prefix => $"{(IsRooted ? "" : "/_")}{(RouteSegment.Length > 0 ? "/" : "")}{RouteSegment}";
        public string GraphQLRoute => $"{Prefix}/graphql";

        public string GraphQLPlaygroundRoute => $"{GraphQLRoute}/ui";
    }
}
