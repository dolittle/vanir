// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Vanir.Backend.Reflection;
using FluentValidation;
using FluentValidation.Results;

namespace Dolittle.Vanir.Backend.Validation
{
    /// <summary>
    /// Represents an implementation of <see cref="IValidators"/>.
    /// </summary>
    public class Validators : IValidators
    {
        readonly ITypes _types;
        readonly IContainer _container;
        readonly Dictionary<Type, List<Type>> _validatorTypesByType = new();

        /// <summary>
        /// Initializes a new instance of <see cref="Validators"/>
        /// </summary>
        /// <param name="types"><see cref="ITypes"/> to use for discovery.</param>
        /// <param name="container"><see cref="IContainer"/> to use for instantiation of validators.</param>
        public Validators(ITypes types, IContainer container)
        {
            _types = types;
            _container = container;

            PopulateValidatorTypesByType();
        }

        /// <inheritdoc/>
        public IEnumerable<Type> All => _validatorTypesByType.SelectMany(kvp => kvp.Value);

        /// <inheritdoc/>
        public bool HasFor(Type type)
        {
            return _validatorTypesByType.ContainsKey(type);
        }

        /// <inheritdoc/>
        public IEnumerable<IValidator> GetFor(Type type)
        {
            return _validatorTypesByType[type].Select(_ => _container.Get(_) as IValidator);
        }

        /// <inheritdoc/>
        public ValidationResult Validate<T>(T instanceToValidate)
        {
            var result = new ValidationResult();
            var validators = GetFor(instanceToValidate.GetType());
            var context = new ValidationContext<T>(instanceToValidate);
            foreach (var validator in validators)
            {
                result = result.MergeWith(validator.Validate(context));
            }

            return result;
        }

        void PopulateValidatorTypesByType()
        {
            foreach (var validatorType in _types.FindMultiple(typeof(AbstractValidator<>)))
            {
                var baseTypes = validatorType.AllBaseAndImplementingTypes();
                var type = baseTypes.Single(_ =>
                {
                    if (_.GenericTypeArguments.Length == 1)
                    {
                        return _ == typeof(AbstractValidator<>).MakeGenericType(_.GenericTypeArguments[0]);
                    }

                    return false;
                });

                if (!_validatorTypesByType.ContainsKey(type.GenericTypeArguments[0]))
                {
                    _validatorTypesByType[type.GenericTypeArguments[0]] = new();
                }
                _validatorTypesByType[type.GenericTypeArguments[0]].Add(validatorType);
            }
        }
    }
}
