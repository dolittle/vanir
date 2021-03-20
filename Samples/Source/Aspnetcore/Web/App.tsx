// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import { PrimaryButton } from '@fluentui/react';
import { withViewModel } from '@dolittle/vanir-react';
import { AppViewModel } from './AppViewModel';

export const App = withViewModel(AppViewModel, ({ viewModel }) => {
    return (
        <>
            <PrimaryButton>Click me</PrimaryButton>
        </>
    );
});
