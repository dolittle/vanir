// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/display-name */

import React, { useState, useCallback, useEffect } from 'react';
import {
    Checkbox,
    CommandBar,
    DetailsList,
    IColumn,
    ICommandBarItemProps,
    Dropdown,
    IDropdownOption,
    Stack,
    Icon,
    SelectionMode
} from '@fluentui/react';

const items: any[] = [
    {
        name: 'my.first.feature',
        description: 'My first feature',
        toggled: true
    }
];

const environmentOptions: IDropdownOption[] = [
    { key: 'local', text: 'Local' },
    { key: 'dev', text: 'Development' },
    { key: 'prod', text: 'Production' }
];

const commandBarItems: ICommandBarItemProps[] = [
    { key: 'apply', text: 'Apply', iconProps: { iconName: 'Save' } },
    {
        key: 'environment',
        text: 'Environment',
        onRender: () => (
            <Stack horizontal style={{ alignItems: 'center' }} >
                <div>Source</div>
                <Dropdown
                    style={{ width: 200 }}
                    placeholder="Select environment"
                    defaultSelectedKey="local"
                    options={environmentOptions} />
            </Stack>
        )
    }
];

const vscode = window.acquireVsCodeApi();

function parseFeatures(text: string) {
    const json = JSON.parse(text);
    const features: any[] = [];

    for (const featureName in json) {
        const feature = json[featureName];
        features.push({
            name: featureName,
            description: feature.description,
            isOn: feature.toggles.some(_ => _.isOn)
        });
    }
    return features;
}


export const FeaturesList = () => {
    const [features, setFeatures] = useState<any[]>([]);

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
        fieldName: 'isOn',
        minWidth: 200,
        onRender: (item, column) => {
            return (
                <Checkbox checked={item.isOn} onChange={(ev, checked) => {
                    const feature = features.find(_ => _ === item);
                    if (feature) {
                        feature.isOn = checked;
                    }
                    setFeatures([...features]);
                }} />
            );
        }
    }];

    const handleMessages = useCallback((event: MessageEvent<any>) => {
        const message = event.data;
        switch (message.type) {
            case 'update': {
                const text = message.text;
                setFeatures(parseFeatures(text));
                vscode.setState({ text });
                return;
            }
        }
    }, []);


    useEffect(() => {
        window.addEventListener('message', handleMessages);
        const state = vscode.getState();
        if (!state) {
            vscode.postMessage({ type: 'updateDocument' });
        } else {
            setFeatures(parseFeatures((state as any).text));
        }
    }, [handleMessages]);

    return (
        <>
            <CommandBar
                styles={{
                    root: {
                        alignItems: 'center',
                    },
                }}
                items={commandBarItems} />
            <DetailsList
                selectionMode={SelectionMode.none}
                columns={columns}
                items={features} />
        </>
    );
};
