// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/prop-types */
import 'reflect-metadata';

import React, { } from 'react';
import ReactDOM from 'react-dom';

import './index.scss';
import { initializeIcons } from '@fluentui/react';
import { FeaturesList } from './Features/FeaturesList';
import { createTheme, loadTheme } from '@fluentui/react/lib/Styling';
import { ApplicationsEditor } from './Applications/ApplicationsEditor';


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

const editor = (document.head.querySelector('[name=editor]') as HTMLMetaElement).content;
let editorToRender: any;
switch (editor) {
    case 'features': {
        editorToRender = <FeaturesList />;
    } break;

    case 'application': {
        editorToRender = <ApplicationsEditor />;
    } break;
}

ReactDOM.render(
    <>
        {editorToRender}
    </>,

    document.getElementById('root')
);

