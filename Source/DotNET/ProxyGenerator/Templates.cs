// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using HandlebarsDotNet;

namespace Dolittle.Vanir.ProxyGenerator
{
    public class Templates : ITemplates
    {
        public HandlebarsTemplate<object, object> MutationTemplate => throw new System.NotImplementedException();

        public HandlebarsTemplate<object, object> QueryTemplate => throw new System.NotImplementedException();

        public HandlebarsTemplate<object, object> TypeTemplate => throw new System.NotImplementedException();

        public Templates()
        {

        }
    }
}
