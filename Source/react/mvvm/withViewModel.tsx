// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/display-name */
import React, { FunctionComponent, useState, useEffect } from 'react';
import { Constructor } from '@dolittle/types';
import { IViewContext } from './IViewContext';
import { ViewModelObserver } from './ViewModelObserver';

import { useParams, useRouteMatch, useHistory } from 'react-router-dom';

export function withViewModel<TViewModel, TProps = {}>(viewModelType: Constructor<TViewModel>, view: FunctionComponent<IViewContext<TViewModel, TProps>>) {
    return (props: TProps) => {
        const params = useParams();
        const history = useHistory();
        const { path, url, isExact } = useRouteMatch();
        const [actualUrl, setActualUrl] = useState(history.location.pathname);

        useEffect(() => {
            const listenerUnregisterCallback = history.listen((location?: any) => setActualUrl(location?.pathname));
            return () => listenerUnregisterCallback();
        });

        return (
            <>
                <ViewModelObserver
                    viewModelType={viewModelType}
                    props={props}
                    view={view}
                    params={params}
                    path={path}
                    matchedUrl={url}
                    isExactMatch={isExact}
                    url={actualUrl} />
            </>
        );
    };
}
