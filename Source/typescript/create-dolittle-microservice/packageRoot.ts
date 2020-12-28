// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';

let root = __dirname;
if (root.endsWith('dist')) {
    root = path.join(root, '..');
}

export default root;
