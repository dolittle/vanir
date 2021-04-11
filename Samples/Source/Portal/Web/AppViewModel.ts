import { INavigator, IMessenger } from '@dolittle/vanir-web';
import { injectable } from 'tsyringe';
import { SearchRequest } from '@shared/portal/search';

@injectable()
export class AppViewModel {
    constructor(
        private readonly _navigator: INavigator,
        private readonly _messenger: IMessenger) {
        _messenger.subscribeTo(SearchRequest, (search) => {
            const path = `/search/${search.query}`;
            _navigator.navigateTo(path);
        });
    }
}
