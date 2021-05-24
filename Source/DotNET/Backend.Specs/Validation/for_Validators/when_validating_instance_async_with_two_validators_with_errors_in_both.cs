// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Reflection;
using Dolittle.Vanir.Backend.Validation;
using FluentValidation;
using FluentValidation.Results;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Dolittle.Vanir.Backend.Validation.for_Validators
{
    public class when_validating_instance_async_with_two_validators_with_errors_in_both
    {
        static Validators validators;
        static Mock<ITypes> types;
        static Mock<IContainer> container;

        static ValidationResult result;

        Establish context = () =>
        {
            types = new();
            container = new();

            types.Setup(_ => _.FindMultiple(typeof(AbstractValidator<>))).Returns(new[] { typeof(FirstValidator), typeof(SecondValidator) });
            container.Setup(_ => _.Get(typeof(FirstValidator))).Returns(new FirstValidator());
            container.Setup(_ => _.Get(typeof(SecondValidator))).Returns(new SecondValidator());
            validators = new Validators(types.Object, container.Object);
        };

        Because of = () => validators.ValidateAsync(new TypeWithValidation()).ContinueWith(_ => result = _.Result).Wait();

        It should_return_result_with_two_errors = () => result.Errors.Count.ShouldEqual(2);
        It should_have_first_validators_message = () => result.Errors[0].ErrorMessage.ShouldEqual(FirstValidator.ErrorMessage);
        It should_have_second_validators_message = () => result.Errors[1].ErrorMessage.ShouldEqual(SecondValidator.ErrorMessage);
    }
}
