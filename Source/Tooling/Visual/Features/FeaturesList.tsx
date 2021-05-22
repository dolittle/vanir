// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import { CommandBar, DetailsList, IColumn, ICommandBarItemProps } from '@fluentui/react';

const columns: IColumn[] = [{
    name: 'Name',
    key: 'Name',
    fieldName: 'name',
    minWidth: 200
}, {
    name: 'Description',
    key: 'Description',
    fieldName: 'description',
    minWidth: 200
}, {
    name: 'State',
    key: 'State',
    fieldName: 'toggled',
    minWidth: 200
}];

const items: any[] = [
    {
        name: 'my.first.feature',
        description: 'My first feature',
        toggled: true
    }
];

const commandBarItems: ICommandBarItemProps[] = [{
    key: 'apply',
    text: 'Apply',
    iconProps: { iconName: 'Save' }
}];

export const FeaturesList = () => {
    return (
        <>
            <CommandBar items={commandBarItems} />
            <DetailsList columns={columns} items={items} />
        </>
    );
    ;
};
