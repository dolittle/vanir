// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using HandlebarsDotNet;

namespace Dolittle.Vanir.ProxyGenerator
{
    public interface ITemplates
    {
        HandlebarsTemplate<object, object> Mutation { get; }
        HandlebarsTemplate<object, object> Query { get; }
        HandlebarsTemplate<object, object> Type { get; }
        HandlebarsTemplate<object, object> Property { get; }
    }
}
