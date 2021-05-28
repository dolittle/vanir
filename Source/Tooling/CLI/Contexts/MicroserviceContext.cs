// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.CLI.Contexts
{
    public class MicroserviceContext
    {
        public ApplicationContext Application { get; init; }
        public Backend.Config.Microservice Microservice { get; init; }
        public string File { get; init; }
        public string Root { get; init; }
        public string DolittleFolder { get; init; }
    }
}
