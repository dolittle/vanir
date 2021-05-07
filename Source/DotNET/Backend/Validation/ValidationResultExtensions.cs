// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using FluentValidation.Results;

namespace Dolittle.Vanir.Backend.Validation
{
    /// <summary>
    /// Extension methods for working with <see cref="ValidationResult"/>
    /// </summary>
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Merge two <see cref="ValidationResult">validation results</see> into a new instance.
        /// </summary>
        /// <param name="left">The first <see cref="ValidationResult"/> </param>
        /// <param name="right">The second <see cref="ValidationResult"/> </param>
        /// <returns>Merged instance of the two <see cref="ValidationResult">validation results</see></returns>
        public static ValidationResult MergeWith(this ValidationResult left, ValidationResult right)
        {
            return new ValidationResult(left.Errors.Concat(right.Errors));
        }
    }
}
