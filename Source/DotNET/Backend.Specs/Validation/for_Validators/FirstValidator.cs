// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentValidation;

namespace Dolittle.Vanir.Backend.Specs.Validation.for_Validators
{
    public class FirstValidator : AbstractValidator<TypeWithValidation>
    {
        public const string ErrorMessage = "FirstValidatorErrorMessage";

        public FirstValidator()
        {
            RuleFor(_ => _.Something).Matches("42").WithMessage(ErrorMessage);
        }
    }
}
