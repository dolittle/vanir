// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/display-name */
import React, { FunctionComponent } from 'react';
import { Constructor } from '@dolittle/types';
import { IViewContext } from './IViewContext';
import { ViewModelObserver } from './ViewModelObserver';

import { useParams, useRouteMatch } from 'react-router-dom';

export function withViewModel<TViewModel, TProps = {}>(viewModelType: Constructor<TViewModel>, view: FunctionComponent<IViewContext<TViewModel, TProps>>) {
    return (props: TProps) => {
        const params = useParams();
        const { path, url } = useRouteMatch();
        return (
            <>
                <ViewModelObserver viewModelType={viewModelType} props={props} view={view} params={params} path={path} url={url} />
            </>
        );
    };
}
