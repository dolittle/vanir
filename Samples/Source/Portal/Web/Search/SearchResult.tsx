import { withViewModel } from '@dolittle/vanir-react';
import React from 'react';
import { Link } from 'react-router-dom';
import { SearchResultViewModel } from './SearchResultViewModel';

export const SearchResult = withViewModel(SearchResultViewModel, ({viewModel}) => {
    return (
        <>
            Hello search


            <Link to="/blah">Something</Link>
        </>
    )
});
