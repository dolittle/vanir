// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import { Toggle } from '@fluentui/react';
import { useFeature, withViewModel } from '@dolittle/vanir-react';
import { AppViewModel } from './AppViewModel';

export const App = withViewModel(AppViewModel, ({ viewModel }) => {
    const materialFeature = useFeature('materials');

    return (
        <>
            <Toggle label="Material feature" checked={materialFeature} onText="On" offText="Off" />
        </>
    );
});
