// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;

namespace Dolittle.Vanir.CLI
{
    public static class ParseResultExtensions
    {
        public static bool HasOption(this ParseResult parseResult, Option option)
        {
            return false;
            //return parseResult.Tokens.Any(_ => _.Type == option.GetType());
        }

    }
}
