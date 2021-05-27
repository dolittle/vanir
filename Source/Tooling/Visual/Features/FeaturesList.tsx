// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/display-name */

import React, { useState, useCallback, useEffect } from 'react';
import { Features, FeaturesParser, IFeatureDefinition } from '@dolittle/vanir-features';
import {
    Checkbox,
    CommandBar,
    DetailsList,
    IColumn,
    ICommandBarItemProps,
    Dropdown,
    IDropdownOption,
    Stack,
    Text,
    Icon,
    SelectionMode,
    getTheme
} from '@fluentui/react';

import { vscode } from '../Globals';

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
        onRender: () => {
            const theme = getTheme();

            return (
                <Stack horizontal style={{ alignItems: 'center' }} tokens={{ childrenGap: 8 }} >
                    <Icon iconName="Source" style={{ color: theme.palette.themePrimary, fontSize: theme.fonts.mediumPlus.fontSize }} />
                    <Text variant="medium">Source</Text>
                    <Dropdown
                        style={{ width: 200 }}
                        placeholder="Select environment"
                        defaultSelectedKey="local"
                        options={environmentOptions} />
                </Stack>
            );
        }
    }
];



function getFeaturesFrom(json: string): IFeatureDefinition[] {
    const parser = new FeaturesParser();
    return parser.parse(json).toDefinitions();
}


export const FeaturesList = () => {
    const [features, setFeatures] = useState<IFeatureDefinition[]>([]);

    const pushChanges = () => {
        const cloned = features.map(_ => ({ ..._}));
        const json = Features.definitionsToJSON([...cloned]);
        vscode.postMessage({ type: 'documentChanged', data: json });
    };


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
        name: 'isOn',
        key: 'isOn',
        minWidth: 200,
        onRender: (item: IFeatureDefinition, column) => {
            return (
                <Checkbox checked={item.toggles[0].isOn} onChange={(ev, checked) => {
                    const feature = features.find(_ => _ === item);
                    if (feature && feature.toggles.length > 0) {
                        feature.toggles[0].isOn = checked!;
                    }
                    setFeatures([...features]);
                    pushChanges();
                }} />
            );
        }
    }];

    const handleMessages = useCallback((event: MessageEvent<any>) => {
        const message = event.data;
        switch (message.type) {
            case 'update': {
                const text = message.text;
                setFeatures(getFeaturesFrom(text));
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
            setFeatures(getFeaturesFrom((state as any).text));
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
