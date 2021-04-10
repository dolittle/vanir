import { IMessenger } from '@dolittle/vanir-web';
import { SearchCleared, SearchRequest } from '@shared/portal/search';

import { Subject, timer } from 'rxjs';
import { debounce } from 'rxjs/operators';
import { injectable } from 'tsyringe';

@injectable()
export class SearchViewModel {
    private _querySubject: Subject<string> = new Subject();
    private _previousQuery = '';

    constructor(private readonly _messenger: IMessenger) {
        const debounced = this._querySubject.pipe(debounce(() => timer(250)));
        debounced.subscribe(this.publishQuery.bind(this));
    }

    queryChanged(query: string) {
        this._querySubject.next(query);
    }

    queryCleared() {
        this._messenger.publish(new SearchCleared());
    }

    private publishQuery(query: string) {
        if (this._previousQuery === '') {
            this.queryCleared();
        } else if (query === '') {
            this.queryCleared();
        }

        this._messenger.publish(new SearchRequest(query));

        this._previousQuery = query;
    }
}
