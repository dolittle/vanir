// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';

import React, { useEffect } from 'react';
import ReactDOM from 'react-dom';

import { Bootstrapper } from '@dolittle/vanir-react';
import { VersionInfo } from '@dolittle/vanir-web';

const version = require('../microservice.json') as VersionInfo;

import '@shared/styles/theme';
import './index.scss';
import { Home } from './Home';
import { useState } from 'react';
import { Route, useHistory, Link } from 'react-router-dom';

export default function App() {
    const [state, setState] = useState(0);

    useEffect(() => {
        const interval = setInterval(() => {
            setState(state + 1);
        }, 1000);

        return () => {
            clearInterval(interval);
        };
    });

    const history = useHistory();


    return (
        <>
            <Route exact path="/">
                <Home something={state.toString()} />
                <Link to="/blah">Click me</Link>
            </Route>
            <Route exact path="/blah">
                Hello there...
                <Link to="/">Go Root</Link>
            </Route>

        </>
    );
}

ReactDOM.render(
    <Bootstrapper name="Portal" prefix="" version={version}>
        <App />
    </Bootstrapper>,
    document.getElementById('root')
);
