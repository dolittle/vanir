// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reactive.Subjects;

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Defines a system that can provide <see cref="Features"/>.
    /// </summary>
    public interface IFeaturesProvider
    {
        /// <summary>
        /// Features <see cref="Subject{T}"/> providing <see cref="Features"/>.
        /// </summary>
        IObservable<Features> Features { get; }
    }
}
