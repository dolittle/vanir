// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { useState, useCallback, useEffect } from 'react';
import ReactFlow, { Node, ArrowHeadType, Connection, ConnectionMode, Controls, Edge } from 'react-flow-renderer';
import { MicroserviceNode } from './MicroserviceNode';
import { vscode } from '../Globals';

const nodeTypes = {
    microservice: MicroserviceNode,
};

export type MicroserviceLayout = {
    left: number;
    top: number;
};

export type EventHorizonLayout = {
    producer: string;
    consumer: string;
    producerHandle: string;
    consumerHandle: string;
};

export type MicroserviceLayouts = { [key: string]: MicroserviceLayout };

export type ApplicationLayout = {
    microservices: MicroserviceLayouts;
    eventHorizonLayouts: EventHorizonLayout[];
};

export type Portal = {
    enabled: boolean;
    id: string;
};

export type EventHorizon = {
    microservice: string;
    tenant: string;
    stream: string;
    partition: string;
    scope: string;
};

export type EventHorizons = { [key: string]: EventHorizon[] };

export type EventHorizonConsent = {
    microservice: string;
    tenant: string;
    stream: string;
    partition: string;
    consent: string;
};

export type EventHorizonConsents = { [key: string]: EventHorizonConsent[] };

export type Microservice = {
    id: string;
    name: string;
    version: string;
    commit: string;
    built: string;
    web: boolean,
    root: string;
    eventHorizons: EventHorizons;
    eventHorizonConsents: EventHorizonConsents;
    eventHorizonsFile: string;
    eventHorizonConsentsFile: string;
    layout: MicroserviceLayout;
};


export type Application = {
    id: string;
    name: string;
    tenant: string;
    license: string;
    containerRegistry: string;
    portal: Portal;
    root: string;
    tenants: string[];
    microservices: Microservice[];
    layout: ApplicationLayout;
};

function createConnectionFrom(producer: string, consumer: string, producerHandle: string, consumerHandle: string): Edge {
    return {
        id: `${producer}${producerHandle}-${consumer}${consumerHandle}`,
        source: producer!,
        sourceHandle: producerHandle!,
        target: consumer!,
        targetHandle: consumerHandle!,
        animated: true,
        label: 'Something',
        labelShowBg: false,
        labelStyle: {
            fill: '#fff',
            fontWeight: 700
        },
        arrowHeadType: ArrowHeadType.Arrow,
        style: {
            stroke: '#fff'
        }
    } as any;
}

function getElementsFrom(json: string): any[] {
    const application = JSON.parse(json) as Application;

    const elements = application.microservices.map(_ => {
        return {
            id: _.id,
            type: 'microservice',
            position: { x: _.layout.left, y: _.layout.top },
            data: { text: _.name },
        };
    });

    if (application.layout && application.layout.eventHorizonLayouts) {
        const connections = application.layout.eventHorizonLayouts.map(_ => {
            return createConnectionFrom(_.producer, _.consumer, _.producerHandle, _.consumerHandle);
        });

        connections.forEach(_ => elements.push(_ as any));
    }

    return elements;
}


export const ApplicationsEditor = () => {
    const [elements, setElements] = useState<Edge[] | Connection[]>([]);

    const onConnect = (connection: Edge | Connection) => {
        const newConnection: Edge = createConnectionFrom(connection.source!, connection.target!, connection.sourceHandle!, connection.targetHandle!);

        vscode.postMessage({
            type: 'connect',
            data: {
                producer: connection.source,
                consumer: connection.target,
                producerHandle: connection.sourceHandle,
                consumerHandle: connection.targetHandle
            }
        });

        setElements([...elements, newConnection as any]);
    };

    const handleMessages = useCallback((event: MessageEvent<any>) => {
        const message = event.data;
        switch (message.type) {
            case 'update': {
                const text = message.text;
                setElements(getElementsFrom(text));
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
            setElements(getElementsFrom((state as any).text));
        }
    }, [handleMessages]);

    const elementMoved = (event: MouseEvent, node: Node) => {
        vscode.postMessage({
            type: 'microserviceMoved', data: {
                id: node.id,
                left: node.position.x,
                top: node.position.y
            }
        });
    };


    return (
        <>
            <ReactFlow
                onConnect={onConnect}
                onNodeDragStop={elementMoved as any}
                connectionMode={ConnectionMode.Loose}
                elements={elements as any[]}
                nodeTypes={nodeTypes}>
                <Controls />
            </ReactFlow>
        </>
    );
};
