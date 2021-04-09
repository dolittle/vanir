// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';

import React, { useEffect } from 'react';
import ReactDOM from 'react-dom';

import { Bootstrapper } from '@dolittle/vanir-react';
import { IMessenger, NavigatedTo, VersionInfo } from '@dolittle/vanir-web';

const version = require('../microservice.json') as VersionInfo;

import '@shared/styles/theme';
import './index.scss';
import { Home } from './Home';
import { useState } from 'react';
import { Route, useHistory, Link } from 'react-router-dom';
import { Stack } from '@fluentui/react';
import { container } from 'tsyringe';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { Observable } from 'rxjs';

import * as H from 'history';

let navigatedToObservable: Observable<NavigatedTo> | undefined

function getNavigatedToObservable() {
    if (!navigatedToObservable) {
        const messenger = container.resolve(IMessenger as constructor<IMessenger>);
        navigatedToObservable = messenger.observe(NavigatedTo);
    }

    return navigatedToObservable;
}


export function useNavigation(): [H.History, H.Location] {
    const history = useHistory();
    const [location, setLocation] = useState(history.location)

    useEffect(() => {
        const subscription = getNavigatedToObservable().subscribe((msg) => {
            setLocation(H.parsePath(msg.path));
        });

        return () => subscription.unsubscribe();
    }, [history]);

    useEffect(() => {
        const unregisterListener = history.listen(() => {
            setLocation(history.location);
        })
        return () => unregisterListener();
    }, [history]);

    return [
        history,
        location
    ];
}


export default function App() {
    const [state, setState] = useState(0);

    /*
    useEffect(() => {
        const interval = setInterval(() => {
            setState(state + 1);
        }, 3000);

        return () => {
            clearInterval(interval);
        };
    });*/

    const [history, location] = useNavigation();
    console.log(`We're at ${location.pathname}`);


    return (
        <>
            <Route exact path="/">
                Hello there...
                <Link to="/blah/42">Click me</Link>
            </Route>
            <Route path="/blah/:something">
                <Home something={state.toString()} />
                <Stack horizontal tokens={{ childrenGap: 5 }}>
                    <Link to="/">Go Root</Link>
                    <Link to="/blah/50">Go to 50</Link>
                    <Link to="/blah/51">Go to 51</Link>
                    <Link to="/blah/51/then">Go to 51 - then</Link>
                    <Link to="/blah/51/some">Go to 51 - some</Link>
                </Stack>
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
