// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.SDK.Events.Filters
{
    /// <summary>
    /// Decorates a method to indicate the <see cref="FilterId"/> of a filter class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PublicFilterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PublicFilterAttribute"/>.
        /// </summary>
        /// <param name="id">The string representation of <see cref="FilterId"/>, which is a <see cref="Guid"/>.</param>
        public PublicFilterAttribute(string id)
        {
            Id = Guid.Parse(id);
        }

        /// <summary>
        /// Gets the unique identifier of the filter.
        /// </summary>
        public FilterId Id { get; }
    }
}
