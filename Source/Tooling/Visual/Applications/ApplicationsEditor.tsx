// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { useState } from 'react';
import ReactFlow, { ArrowHeadType, Connection, ConnectionMode, Controls, Edge } from 'react-flow-renderer';
import { MicroserviceNode } from './MicroserviceNode';

const initialElements = [
    {
        id: '1',
        type: 'microservice',
        position: { x: 0, y: 0 },
        data: { text: 'Flattened JSON Producer' },
    },
    {
        id: '2',
        type: 'microservice',
        position: { x: 50, y: 200 },
        data: { text: 'eCommerce' },
    },

    {
        id: '3',
        type: 'microservice',
        position: { x: 600, y: 400 },
        data: { text: 'Product Structure Extractor' },
    },

    {
        id: '4',
        type: 'microservice',
        position: { x: 300, y: 500 },
        data: { text: 'Portal' },
    }

    /*,
    { id: 'e1-2', source: '2', target: '1', animated: true, arrowHeadType: ArrowHeadType.ArrowClosed },
    { id: 'e3-2', source: '3', target: '2', animated: true, arrowHeadType: ArrowHeadType.ArrowClosed },
    { id: 'e2-2', source: '5', target: '6', sourceHandle: 'a', targetHandle: 'a', animated: true },*/
];

const nodeTypes = {
    microservice: MicroserviceNode,
};


export const ApplicationsEditor = () => {
    const [elements, setElements] = useState(initialElements);

    const onConnect = (connection: Edge | Connection) => {
        const newConnection = {
            id: `${connection.source}${connection.sourceHandle}-${connection.target}${connection.targetHandle}`,
            animated: true,
            arrowHeadType: ArrowHeadType.ArrowClosed
        };

        setElements([...elements, {...newConnection, ... connection} as any]);
    };

    return (
        <>
            <ReactFlow
                onConnect={onConnect}
                connectionMode={ConnectionMode.Strict}
                elements={elements}
                nodeTypes={nodeTypes}>
                <Controls />
            </ReactFlow>
        </>
    );
};
