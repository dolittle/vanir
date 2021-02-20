// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * Represents the actual configuration of a resource type.
 */
export type ResourceTypeStructure = {
    [key: string]: any;
};

/**
 * Represents the configuration of all resource types in a resources.json file
 */
export type ResourcesFileStructure = {
    [key: string]: ResourceTypeStructure;
};
