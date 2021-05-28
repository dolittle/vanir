// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { useState } from 'react';
import { withViewModel } from '@dolittle/vanir-react';
import { AppViewModel } from './AppViewModel';
import { useFeature } from '@dolittle/vanir-react';

import { Toggle } from '@fluentui/react';

export const App = withViewModel(AppViewModel, ({ viewModel }) => {
    const cartFeature = useFeature('cart');

    return (
        <>
            <Toggle label="cart" checked={cartFeature} onText="On" offText="Off" />
        </>
    );
});
