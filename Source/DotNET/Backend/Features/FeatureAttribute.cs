// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Attribute used to adorn classes or methods as part of a feature.
    /// </summary>
    /// <remarks>
    /// Different pipelines can take advantage of this.
    /// Features registered here are typically then available through the <see cref="IFeatureToggles"/>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FeatureAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FeatureAttribute"/>.
        /// </summary>
        /// <param name="name">Name of feature.</param>
        public FeatureAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
