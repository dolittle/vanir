// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Dolittle.Vanir.Backend.Concepts;
using HotChocolate.Utilities;

namespace Dolittle.Vanir.Backend.GraphQL.Concepts
{
    public class ConceptAsTypeProvider<TConcept> : IChangeTypeProvider
    {
        public bool TryCreateConverter(Type source, Type target, ChangeTypeProvider root, [NotNullWhen(true)] out ChangeType converter)
        {
            if (source == typeof(TConcept))
            {
                var conceptValueType = source.GetConceptValueType();
                if (source == typeof(TConcept) && target == conceptValueType)
                {
                    converter = (value) => value.GetConceptValue();
                    return true;
                }
            }

            if (target == typeof(TConcept))
            {
                var conceptValueType = target.GetConceptValueType();
                if (source == conceptValueType && target == typeof(TConcept))
                {
                    converter = (value) => ConceptFactory.CreateConceptInstance(typeof(TConcept), value);
                    return true;
                }
            }

            converter = null;
            return false;
        }
    }
}
