// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export class PathHelper {
    /**
     * Force path separator to be what is expected on Unix systems.
     *
     * The reason for this helper came from Plop having problems on Windows finding files in the template
     * folder. This stems from Plop using a package called Globby for discovering files and it having a problem
     * on windows with the forward slash used on Windows paths. So any path.* methods being used, which normalizes
     * the path to be for the target platform will make a path that Globby didn't like.
     *
     * Read more here: https://github.com/sindresorhus/globby/issues/155
     *
     * @param {string} path Path to change
     * @returns Path with path separator set to what is expected on unix based systems
     */
    static useUnixPathSeparator(path: string) {
        return path.replace(/\\/g, '/');
    }
}

