// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentValidation;

namespace Dolittle.Vanir.Backend.Validation.for_Validators
{
    public class SecondValidator : AbstractValidator<TypeWithValidation>
    {
        public const string ErrorMessage = "SecondValidatorErrorMessage";
        public SecondValidator()
        {
            RuleFor(_ => _.Something).Matches("43").WithMessage(ErrorMessage);
        }
    }
}
