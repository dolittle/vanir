// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace Dolittle.Vanir.Backend.Validation
{
    /// <summary>
    /// Defines a system for working with validators.
    /// </summary>
    public interface IValidators
    {
        /// <summary>
        /// Gets all validators in the system.
        /// </summary>
        IEnumerable<Type> All { get; }

        /// <summary>
        /// Check if there is a validator for a type.
        /// </summary>
        /// <param name="type">Type to check for</param>
        /// <returns>True if there are validators, false if not</returns>
        bool HasFor(Type type);

        /// <summary>
        /// Get validators for a specific type.
        /// </summary>
        /// <param name="type">Type to get for</param>
        /// <returns>All <see cref="IEnumerable(T}">validators</see> for type</returns>
        IEnumerable<IValidator> GetFor(Type type);

        /// <summary>
        /// Validate an instance.
        /// </summary>
        /// <param name="instanceToValidate">Instance to validate.</param>
        /// <returns>The <see cref="ValidationResult">result</see> of the validation.</returns>
        ValidationResult Validate<T>(T instanceToValidate);
    }
}
