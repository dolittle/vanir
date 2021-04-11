import { withViewModel } from '@dolittle/vanir-react';
import React from 'react';
import { Layout } from './Layout';
import { AppViewModel } from './AppViewModel';

export const App = withViewModel(AppViewModel, ({ viewModel }) => {
    return (
        <>
            <Layout />
        </>
    );
});
