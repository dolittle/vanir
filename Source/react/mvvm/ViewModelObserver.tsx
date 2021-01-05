// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/no-direct-mutation-state */
import React, { FunctionComponent } from 'react';
import { BehaviorSubject } from 'rxjs';
import { ViewModelHelpers } from './ViewModelHelpers';
import { IViewContext } from './IViewContext';
import { Constructor } from '@dolittle/types';
import { constructor, IContainer } from '@dolittle/vanir-dependency-inversion';
import { container } from 'tsyringe';
import { IViewModelLifecycleManager } from './IViewModelLifecycleManager';

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

    private initialize() {

        const viewModel = this._viewModelLifecycleManager.create<TViewModel>(this.props.viewModelType);

        ViewModelHelpers.bindViewModelFunctions(viewModel);

        const viewContext = {
            viewModel,
            props: this.props.props,
            view: this.props.view,
            params: this.props.params,
            path: this.props.path,
            url: this.props.url
        } as IViewContext<TViewModel, TProps>;

        const routeInfo = {
            url: this.props.url,
            path: this.props.path,
            params: this.props.params
        };

        this._viewModelLifecycleManager.attached(viewModel, routeInfo);
        this._viewModelLifecycleManager.propsChanged(viewModel, this.props.props);
        this._viewModelLifecycleManager.paramsChanged(viewModel, this.props.params, routeInfo);

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

        for (const key in prevProps.params) {
            if (prevProps.params[key] !== this.props.params[key]) {

                const routeInfo = {
                    url: this.props.url,
                    path: this.props.path,
                    params: this.props.params
                };

                this._viewModelLifecycleManager.paramsChanged(this.state.viewModel, this.props.params, routeInfo);
                break;
            }
        }
    }

    private mutate() {
        this.forceUpdate();
    }

    render() {
        return (
            <>
                {React.createElement(this.props.view, this.state)}
            </>
        );
    }
}
