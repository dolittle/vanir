import React from 'react';
import { withViewModel } from '@dolittle/vanir-react';
import { BlahViewModel } from './BlahViewModel';

export const Blah = withViewModel(BlahViewModel, ({ viewModel }) => {
    return (
        <>
            This is blah...
        </>
    );
});
