// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reactive.Subjects;

namespace Dolittle.Vanir.Backend.Features
{

    /// <summary>
    /// Represents an implementation of <see cref="IFeaturesProvider"/>
    /// </summary>
    public class FeaturesProvider : IFeaturesProvider
    {
        static readonly string _featuresPath = Path.Combine(".dolittle","features.json");
        readonly BehaviorSubject<Features> _features = new(new Features());
        readonly IFeaturesParser _featuresParser;

        /// <summary>
        /// Initializes a new instance of <see cref="FeaturesProvider"/>
        /// </summary>
        /// <param name="featuresParser"><see cref="IFeaturesParser"/> for parsing from JSON.</param>
        public FeaturesProvider(IFeaturesParser featuresParser)
        {
            _featuresParser = featuresParser;

            var watcher = new FileSystemWatcher(Path.GetDirectoryName(_featuresPath));
            watcher.Changed += (s, e) => LoadFeatures();
            watcher.Created += (s, e) => LoadFeatures();
            watcher.Deleted += (s, e) => LoadFeatures();
            watcher.NotifyFilter =
                        NotifyFilters.Attributes |
                        NotifyFilters.CreationTime |
                        NotifyFilters.DirectoryName |
                        NotifyFilters.FileName |
                        NotifyFilters.LastAccess |
                        NotifyFilters.LastWrite |
                        NotifyFilters.Security |
                        NotifyFilters.Size;
            watcher.EnableRaisingEvents = true;

            LoadFeatures();
        }

        /// <inheritdoc/>
        public IObservable<Features> Features => _features;

        void LoadFeatures()
        {
            if (!File.Exists(_featuresPath))
            {
                _features.OnNext(new Features());
                return;
            }
            var featuresAsJson = File.ReadAllText(_featuresPath);
            var features = _featuresParser.Parse(featuresAsJson);
            _features.OnNext(new Features(features));
        }
    }
}
