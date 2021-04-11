import React from 'react';

import { SearchBox } from '@fluentui/react';
import { withViewModel } from '@dolittle/vanir-react';

import './Search.scss';
import { SearchViewModel } from './SearchViewModel';

export const Search = withViewModel(SearchViewModel, ({ viewModel }) => {
    return (
        <div className="search">
            <SearchBox underlined
                placeholder="Search"
                onClear={viewModel.queryCleared}
                onChange={(e, v) => viewModel.queryChanged(v || '')} />
        </div>
    );
});