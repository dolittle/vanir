// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/prop-types */
import 'reflect-metadata';

import React, { FC } from 'react';
import ReactDOM from 'react-dom';
import ReactFlow, { ArrowHeadType, Controls, Handle, MiniMap, Node, Position } from 'react-flow-renderer';

import { DocumentCard, DocumentCardActions, DocumentCardTitle, initializeIcons, PrimaryButton } from '@fluentui/react';

import './index.scss';

const elements = [
    { id: '1', data: { label: 'Flattened JSON Producer' }, position: { x: 250, y: 5 } },
    // you can also pass a React component as a label
    { id: '2', data: { label: <div>eCommerce</div> }, position: { x: 100, y: 100 } },
    { id: '3', data: { label: <div>Product Structure Extractor</div> }, position: { x: 200, y: 100 } },
    { id: '4', data: { label: <div>Portal</div> }, position: { x: 100, y: 200 } },
    { id: 'e1-2', source: '2', target: '1', animated: true, arrowHeadType: ArrowHeadType.ArrowClosed },
    { id: 'e3-2', source: '3', target: '2', animated: true, arrowHeadType: ArrowHeadType.ArrowClosed },
    {
        id: '5',
        type: 'special',
        position: { x: 300, y: 100 },
        data: { text: 'Something' },
    },

    {
        id: '6',
        type: 'special',
        position: { x: 400, y: 100 },
        data: { text: 'Another node' },
    },
    { id: 'e2-2', source: '5', target: '6', sourceHandle: 'a', targetHandle: 'a', animated: true },
];


/*
https://reactnativeexample.com/an-animated-and-customizable-circular-floating-menu/
https://antho2407.github.io/react-radial-menu/
https://fabiobiondi.github.io/react-circular-menu/

https://codepen.io/logrithumn/pen/yMwYXX

https://codepen.io/lenymo/pen/rwmBGq

https://codepen.io/maskedcoder/pen/zqgpr

https://codepen.io/mahmoud-nb/pen/pbNBYP
*/
const onActionClick = (action: string, ev: React.SyntheticEvent<HTMLElement>): void => {
    console.log(`You clicked the ${action} action`);
    ev.stopPropagation();
    ev.preventDefault();
};

const documentCardActions = [
    {
        iconProps: { iconName: 'Share' },
        ariaLabel: 'share action',
    },
    {
        iconProps: { iconName: 'Pin' },
        ariaLabel: 'pin action',
    },
    {
        iconProps: { iconName: 'Ringer' },
        ariaLabel: 'notifications action',
    },
];

const customNodeStyles = {
    background: '#9CA8B3',
    color: '#FFF',
    padding: 0,
};

initializeIcons();

const CustomNodeComponent: FC<Node> = ({ data }) => {


    return (
        <div style={customNodeStyles}>
            <Handle id="a" type="source" position={Position.Top} style={{ borderRadius: 0 }} />

            <DocumentCard>
                <DocumentCardTitle title={data.text} />
                <DocumentCardActions actions={documentCardActions} />
            </DocumentCard>
            <Handle
                type="source"
                position={Position.Left}
                id="b"
                style={{ borderRadius: 0 }}
            />
            <Handle
                type="target"
                position={Position.Right}
                id="c"
                style={{ borderRadius: 0 }}
            />
            <Handle id="d" type="source" position={Position.Bottom} style={{ borderRadius: 0 }} />
        </div>
    );
};

const nodeTypes = {
    special: CustomNodeComponent,
};

ReactDOM.render(

    <ReactFlow elements={elements} nodeTypes={nodeTypes}>
        <Controls />
    </ReactFlow>,
    document.getElementById('root')
);


// <MiniMap />
