// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using FluentValidation.Results;
using Machine.Specifications;

namespace Dolittle.Vanir.Backend.Validation.for_ValidationResultExtensions
{
    public class when_merging_two_validation_results_with_errors
    {
        static ValidationResult first;
        static ValidationResult second;
        static ValidationResult result;

        Establish context = () =>
        {
            first = new ValidationResult(new[] {
                new ValidationFailure("something", "It is all wrong")
            });

            second = new ValidationResult(new[] {
                new ValidationFailure("somethingElse", "It is all wrong")
            });
        };

        Because of = () => result = first.MergeWith(second);

        It should_result_in_a_validation_result_with_errors_from_both = () => result.Errors.ShouldContain(first.Errors.Concat(second.Errors));
    }
}
