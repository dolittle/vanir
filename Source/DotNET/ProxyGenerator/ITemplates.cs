// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using HandlebarsDotNet;

namespace Dolittle.Vanir.ProxyGenerator
{
    public interface ITemplates
    {
        HandlebarsTemplate<object, object> MutationTemplate { get; }
        HandlebarsTemplate<object, object> QueryTemplate { get; }
        HandlebarsTemplate<object, object> TypeTemplate { get; }
    }
}
