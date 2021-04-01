// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Attribute to use for adorning methods indicating it represents a query on a <see cref="GraphController"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryAttribute : Attribute, ICanHavePath
    {
        /// <summary>
        /// Initializes a new instance of <see cref="QueryAttribute"/>
        /// </summary>
        /// <param name="path">Optional path of the query - for overriding the default picked from the method name</param>
        public QueryAttribute(string path = null)
        {
            Path = path;
        }

        /// <inheritdoc/>
        public string Path { get; }

        /// <inheritdoc/>
        public bool HasPath => Path is not null;
    }
}
