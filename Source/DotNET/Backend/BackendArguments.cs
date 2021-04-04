// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.SDK;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public class BackendArguments
    {
        public ILoggerFactory LoggerFactory { get; set; } = new NullLoggerFactory();
        public Action<IRequestExecutorBuilder> GraphQLExecutorBuilder = (b) => {};
        public Action<ClientBuilder> DolittleClientBuilderCallback = (b) => {};
    }
}
