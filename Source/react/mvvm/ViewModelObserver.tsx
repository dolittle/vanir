// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/no-direct-mutation-state */
import React, { FunctionComponent } from 'react';
import { BehaviorSubject } from 'rxjs';
import { ViewModelHelpers } from './ViewModelHelpers';
import { IViewContext } from './IViewContext';
import { Constructor } from '@dolittle/types';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { container } from 'tsyringe';
import { IViewModelLifecycleManager } from './IViewModelLifecycleManager';
import { RouteInfo } from './RouteInfo';

export type ViewModelObserverProps<TViewModel, TProps = {}> = {
    view: FunctionComponent<IViewContext<TViewModel, TProps>>,
    viewModelType: Constructor<TViewModel>
    props: TProps,
    params: any,
    path: string,
    url: string
};

export class ViewModelObserver<TViewModel, TProps = {}> extends React.Component<ViewModelObserverProps<TViewModel, TProps>, IViewContext<TViewModel, TProps>> {
    private _viewModelLifecycleManager: IViewModelLifecycleManager;

    constructor(props: ViewModelObserverProps<TViewModel, TProps>) {
        super(props);
        this._viewModelLifecycleManager = container.resolve<IViewModelLifecycleManager>(IViewModelLifecycleManager as constructor<IViewModelLifecycleManager>);
        this.initialize();
    }

    componentDidMount() {
    }

    componentWillUnmount() {
        this._viewModelLifecycleManager.detached(this.state.viewModel);
    }


    componentDidUpdate(prevProps: ViewModelObserverProps<TViewModel, TProps>) {
        for (const key in prevProps.props) {
            if (prevProps.props[key] !== this.props.props[key]) {
                this.setState({ props: this.props.props });
                this._viewModelLifecycleManager.propsChanged(this.state.viewModel, this.props.props);
                break;
            }
        }

        const routeInfo: RouteInfo = {
            url: this.props.url,
            path: this.props.path,
            params: this.props.params
        };
        let routeChanged = prevProps.url !== routeInfo.url;

        const prevParamsKeys = Object.keys(prevProps.params);
        const curParamsKeys = Object.keys(this.props.params);

        if (!curParamsKeys.some(_ => prevParamsKeys.some(k => k === _))) {
            routeChanged = true;
        }
        if (!prevParamsKeys.some(_ => curParamsKeys.some(k => k === _))) {
            routeChanged = true;
        }

        for (const key in this.props.params) {
            if (prevProps.params[key] !== routeInfo.params[key]) {
                routeChanged = true;
                break;
            }
        }

        if (routeChanged) {
            this._viewModelLifecycleManager.routeChanged(this.state.viewModel, routeInfo);
        }
    }

    render() {
        return (
            <>
                {React.createElement(this.props.view, this.state)}
            </>
        );
    }

    private mutate() {
        this.forceUpdate();
    }

    private initialize() {

        const viewModel = this._viewModelLifecycleManager.create<TViewModel>(this.props.viewModelType);

        ViewModelHelpers.bindViewModelFunctions(viewModel);

        const routeInfo = {
            url: this.props.url,
            path: this.props.path,
            params: this.props.params
        };

        const viewContext = {
            viewModel,
            props: this.props.props,
            view: this.props.view,
            routeInfo
        } as IViewContext<TViewModel, TProps>;

        console.log('Hello');

        this._viewModelLifecycleManager.attached(viewModel, routeInfo);
        this._viewModelLifecycleManager.propsChanged(viewModel, this.props.props);
        this._viewModelLifecycleManager.routeChanged(viewModel, routeInfo);

        const viewModelAsAny = viewModel as any;

        const properties = ViewModelHelpers.getNonInternalPropertiesFrom(viewModel);
        for (const property of properties) {
            const observablePropertyName = ViewModelHelpers.getObservablePropertyNameFor(property);
            const propertyValue = viewModelAsAny[property];
            viewModelAsAny[observablePropertyName] = new BehaviorSubject(propertyValue);

            Object.defineProperty(viewModel, property, {
                get: () => {
                    return viewModelAsAny[observablePropertyName].value;
                },
                set: (value: any) => {
                    viewModelAsAny[observablePropertyName].next(value);
                }
            });
        }

        ViewModelHelpers.setSubscriptionsOn(viewModel, () => {
            this.mutate();
        });

        this.state = viewContext;
    }

}
