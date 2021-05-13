// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/prop-types */
import 'reflect-metadata';

import React, { FC, useState } from 'react';
import ReactDOM from 'react-dom';
import ReactFlow, { ArrowHeadType, Controls, Handle, MiniMap, Node, Position } from 'react-flow-renderer';

import { DocumentCard, DocumentCardActions, DocumentCardLogo, DocumentCardTitle, DocumentCardType, IButtonProps, initializeIcons, PrimaryButton } from '@fluentui/react';

import './index.scss';
import { createTheme, loadTheme } from '@fluentui/react/lib/Styling';
import * as PieTypes from './menu/PieTypes';
import { CircularMenu } from './menu/CircularMenu';
import { CircularMenuConfig } from './menu/CircularMenuConfig';

/*
https://reactnativeexample.com/an-animated-and-customizable-circular-floating-menu/
https://antho2407.github.io/react-radial-menu/
https://fabiobiondi.github.io/react-circular-menu/

https://codepen.io/logrithumn/pen/yMwYXX

https://codepen.io/lenymo/pen/rwmBGq

https://codepen.io/maskedcoder/pen/zqgpr

https://codepen.io/mahmoud-nb/pen/pbNBYP
*/

//const vscode = window.acquireVsCodeApi();

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


const CustomNodeComponent: FC<Node> = ({ data }) => {


    return (
        <div style={customNodeStyles}>
            <Handle id="a" type="source" position={Position.Top} style={{ borderRadius: 0 }} />

            <DocumentCard type={DocumentCardType.normal}>
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

initializeIcons();

const myTheme = createTheme({
    palette: {
        themePrimary: '#ffcf00',
        themeLighterAlt: '#fffdf5',
        themeLighter: '#fff8d6',
        themeLight: '#fff1b3',
        themeTertiary: '#ffe366',
        themeSecondary: '#ffd61f',
        themeDarkAlt: '#e6bb00',
        themeDark: '#c29e00',
        themeDarker: '#8f7500',
        neutralLighterAlt: '#292d32',
        neutralLighter: '#292c31',
        neutralLight: '#272a2f',
        neutralQuaternaryAlt: '#24282c',
        neutralQuaternary: '#23262a',
        neutralTertiaryAlt: '#212428',
        neutralTertiary: '#c8c8c8',
        neutralSecondary: '#d0d0d0',
        neutralPrimaryAlt: '#dadada',
        neutralPrimary: '#ffffff',
        neutralDark: '#f4f4f4',
        black: '#f8f8f8',
        white: '#2b2f34',
    }
});

loadTheme(myTheme);


const menu = 0;

const menus = {
    CONTRAST: {
        data: [{ label: '', icon: '', value: 20, bg: 'white', color: '#512DA8', type: 'ROOT' }, { label: 'contrast', icon: '', value: 20, bg: '#9575CD', color: 'white', type: 'ROOT' }, { label: 'battery', icon: '', value: 20, bg: '#7E57C2', color: 'white', type: 'ROOT' }, { label: 'bluetooth', icon: '', value: 20, bg: '#673AB7', color: 'white', type: 'ROOT' }, { label: 'light', icon: '', value: 20, bg: '#5E35B1', color: 'white', type: 'ROOT' }, { label: 'settings', icon: '', value: 20, bg: '#512DA8', color: 'white', type: 'ROOT' }, { label: 'contrast', icon: '', value: 20, bg: '#9575CD', color: 'white', type: 'ROOT' }, { label: 'View', icon: '', value: 20, bg: '#7E57C2', color: 'white', type: 'ROOT' }, { label: 'bluetooth', icon: '', value: 20, bg: '#673AB7', color: 'white', type: 'ROOT' }, { label: 'light', icon: '', value: 20, bg: '#5E35B1', color: 'white', type: 'ROOT' }],
        config: {
            type: PieTypes.CIRCLE,
            colors: [],
            width: null,
            showIcon: true,
            sizeIcon: '1.6em',
            pieSize: 70
        }
    },

    BATTERY: {
        data: [{ label: '', icon: '', value: 20, bg: 'white', color: 'rgb(121, 121, 121)', type: 'ROOT' }, { label: 'contrast', icon: '', value: 20, bg: 'rgb(179, 179, 179)', color: '#222', type: 'ROOT' }, { label: 'battery', icon: '', value: 20, bg: 'rgb(140, 140, 140)', color: '#222', type: 'ROOT' }, { label: 'bluetooth', icon: '', value: 20, bg: 'rgb(133, 133, 133)', color: '#222', type: 'ROOT' }],
        config: {
            type: PieTypes.CIRCLE,
            colors: [],
            width: null,
            showIcon: true,
            sizeIcon: '1.2m',
            pieSize: 70
        }
    },

    LIGHTS: {
        data: [{ label: '', icon: '', value: 20, bg: 'white', color: 'rgb(193, 186, 33)', type: 'ROOT' }, { label: 'contrast', icon: '', value: 20, bg: 'rgb(238, 232, 85)', color: '#bdac18', type: 'ROOT' }, { label: 'battery', icon: '', value: 20, bg: 'rgb(246, 242, 130)', color: '#bdac18', type: 'ROOT' }, { label: 'bluetooth', icon: '', value: 20, bg: 'rgb(238, 232, 85)', color: '#bdac18', type: 'ROOT' }],
        config: {
            type: PieTypes.CIRCLE,
            colors: [],
            width: null,
            showIcon: true,
            sizeIcon: '1.6em',
            pieSize: 70
        }
    },

    SETTINGS: {
        data: [{ label: '', icon: '', value: 20, bg: 'white', color: 'rgb(121, 121, 121)', type: 'ROOT' }, { label: 'contrast', icon: '', value: 20, bg: 'rgb(179, 179, 179)', color: '#222', type: 'ROOT' }, { label: 'battery', icon: '', value: 20, bg: 'rgb(140, 140, 140)', color: '#222', type: 'ROOT' }, { label: 'bluetooth', icon: '', value: 20, bg: 'rgb(133, 133, 133)', color: '#222', type: 'ROOT' }],
        config: {
            type: PieTypes.CIRCLE,
            colors: [],
            width: null,
            showIcon: true,
            sizeIcon: '1.2m',
            pieSize: 70
        }
    },

    BLUETOOTH: {
        data: [{ label: 'enable', value: 70, bg: 'rgb(57, 172, 221)', color: 'white', type: 'ROOT' }, { label: 'back', value: 30, bg: 'white', color: 'rgb(57, 172, 221)', type: 'ROOT' }],
        config: {
            type: PieTypes.CIRCLE,
            colors: [],
            width: null,
            showIcon: false,
            sizeIcon: '1em',
            pieSize: 100
        }
    },

    ROOT: {
        data: [{ label: 'contrast', icon: '', value: 20, bg: '#9575CD', color: 'white', type: 'CONTRAST' }, { label: 'battery', icon: '', value: 20, bg: 'grey', color: 'white', type: 'BATTERY' }, { label: 'bluetooth', icon: '', value: 20, bg: 'rgb(142, 186, 223)', color: 'white', type: 'BLUETOOTH' }, { label: 'light', icon: '', value: 20, bg: 'rgb(242, 224, 131)', color: 'white', type: 'LIGHTS' }, { label: 'settings', icon: '', value: 20, bg: 'rgb(201, 143, 110)', color: 'white', type: 'SETTINGS' }],
        config: {
            type: PieTypes.CIRCLE,
            colors: [],
            width: null,
            showIcon: true,
            sizeIcon: '1.2em',
            pieSize: 50,
            showCenteredLabel: false
        }
    }
};

const Menu = () => {
    const [menu, setMenu] = useState(menus.ROOT);


    return (
        <>
            <CircularMenu
                config={menu.config as any}
                data={menu.data} style={{ width: '50%', height: '50%' }}
                onItemClick={(d) => {
                    setMenu(menus[d.type]);
                }}
            />
        </>
    );
};



ReactDOM.render(
    <>
        <div style={{ width: '400px', height: '400px' }}>
            <Menu />
        </div>
    </>,

    document.getElementById('root')
);


// <MiniMap />
/*
        <ReactFlow elements={elements} nodeTypes={nodeTypes}>
            <Controls />
        </ReactFlow>

*/
