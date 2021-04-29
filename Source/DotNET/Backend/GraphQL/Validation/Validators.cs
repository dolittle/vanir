// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Vanir.Backend.Reflection;
using FluentValidation;

namespace Dolittle.Vanir.Backend.GraphQL.Validation
{
    public class Validators : IValidators
    {
        readonly ITypes _types;
        readonly IContainer _container;
        readonly Dictionary<Type, List<Type>> _validatorTypesByType = new();

        public Validators(ITypes types, IContainer container)
        {
            _types = types;
            _container = container;

            PopulateValidatorTypesByType();
        }

        public IEnumerable<Type> All => _validatorTypesByType.SelectMany(kvp => kvp.Value);

        public bool HasFor(Type type)
        {
            return _validatorTypesByType.ContainsKey(type);
        }

        public IEnumerable<IValidator> GetFor(Type type)
        {
            return _validatorTypesByType[type].Select(_ => _container.Get(_) as IValidator);
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
