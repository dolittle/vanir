import { withViewModel, RouteInfo } from '@dolittle/vanir-react';
import { IMessenger, INavigator } from '@dolittle/vanir-web';
import React from 'react';
import { injectable } from 'tsyringe';
import { SearchRequest } from '@shared/portal/search';


type Params = {
    q: string;
}

@injectable()
export class AppViewModel {

    constructor(
        private readonly _messenger: IMessenger,
        private readonly _navigator: INavigator
    ) { }

    attached(routeInfo: RouteInfo): void {
        this._messenger.subscribeTo(
            SearchRequest,
            (request) => {
                this.navigateToSearch(request.query.trim());
            }
        );
    }

    routeChanged(routeInfo: RouteInfo<Params>): void {
    }

    navigateToSearch(query) {
        if (query) {
            this._navigator.navigateTo(`/typescript/search/${query}`);
        }
    }

}

export const App = withViewModel(AppViewModel, ({ viewModel }) => {
    return (
        <>
            This is it...
        </>
    );
});
