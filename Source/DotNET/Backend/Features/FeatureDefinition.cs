// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.Features
{
    public class FeatureDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public FeatureToggleDefinition[] Toggles { get; set; }
    }
}
