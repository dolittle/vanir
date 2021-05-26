// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import ReactFlow, { ArrowHeadType, Controls } from 'react-flow-renderer';
import { MicroserviceNode } from './MicroserviceNode';

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
        type: 'microservice',
        position: { x: 300, y: 100 },
        data: { text: 'Something' },
    },

    {
        id: '6',
        type: 'microservice',
        position: { x: 400, y: 100 },
        data: { text: 'Another node' },
    },
    { id: 'e2-2', source: '5', target: '6', sourceHandle: 'a', targetHandle: 'a', animated: true },
];

const nodeTypes = {
    microservice: MicroserviceNode,
};


export const ApplicationsEditor = () => {
    return (
        <>
            <ReactFlow elements={elements} nodeTypes={nodeTypes}>
                <Controls />
            </ReactFlow>
        </>
    );
};
