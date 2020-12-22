// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { VersionInfo } from './VersionInfo';

export interface Configuration {
    name: string;
    prefix: string;
    versionInfo: VersionInfo;
}
