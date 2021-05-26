// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { FC, useState } from 'react';
import { Handle, Node, Position } from 'react-flow-renderer';
import { DocumentCard, DocumentCardActions, DocumentCardTitle, DocumentCardType, IButtonProps } from '@fluentui/react';

const onActionClick = (): void => {
    //vscode.postMessage({ type: 'hello' });

    //ev.stopPropagation();
    //ev.preventDefault();
};


const documentCardActions: IButtonProps[] = [
    {
        iconProps: { iconName: 'Play' },
        ariaLabel: 'Start',
        title: 'Start',
    },
    {
        iconProps: { iconName: 'AllApps' },
        onClick: onActionClick,
        ariaLabel: 'Features',
        title: 'Features',
    },
    {
        iconProps: { iconName: 'BullseyeTarget' },
        ariaLabel: 'Event Horizon',
        title: 'Event Horizon',
    },
    {
        iconProps: { iconName: 'SearchData' },
        ariaLabel: 'Event Store',
        title: 'Event Store',
    },
];

const customNodeStyles = {
    background: '#9CA8B3',
    color: '#FFF',
    padding: 0,
};


export const MicroserviceNode: FC<Node> = ({ data }) => {

    return (
        <div style={customNodeStyles}>

            <DocumentCard type={DocumentCardType.normal}>
                <DocumentCardTitle title={data.text} />
                <DocumentCardActions actions={documentCardActions} />
            </DocumentCard>

            <Handle id="a-source" type="source" position={Position.Top} style={{ borderRadius: 5 }} />
            <Handle id="b-source" type="source" position={Position.Left} style={{ borderRadius: 5 }} />
            <Handle id="c-target" type="target" position={Position.Right} style={{ borderRadius: 0 }} />
            <Handle id="d-target" type="target" position={Position.Bottom} style={{ borderRadius: 0 }} />
        </div>
    );

};
