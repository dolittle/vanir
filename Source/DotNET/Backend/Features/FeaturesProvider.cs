// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using Newtonsoft.Json;

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Represents an implementation of <see cref="IFeaturesProvider"/>
    /// </summary>
    public class FeaturesProvider : IFeaturesProvider
    {
        const string _featuresPath = "./data/features.json";
        readonly BehaviorSubject<Features> _features = new(new Features());

        public FeaturesProvider()
        {
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

        void LoadFeatures()
        {
            if (!File.Exists(_featuresPath))
            {
                _features.OnNext(new Features());
                return;
            }
            var featuresAsJson = File.ReadAllText(_featuresPath);
            var featureBooleans = JsonConvert.DeserializeObject<Dictionary<string, bool>>(featuresAsJson);
            var features = featureBooleans.ToDictionary(
                                _ => _.Key,
                                _ => new BooleanFeatureToggleStrategy { IsOn = _.Value } as IFeatureToggleStrategy);
            _features.OnNext(new Features(features));
        }

        public IObservable<Features> Features => _features;
    }
}
