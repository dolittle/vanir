// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import { Toggle } from '@fluentui/react';
import { useFeature, withViewModel } from '@dolittle/vanir-react';
import { AppViewModel } from './AppViewModel';

export const App = withViewModel(AppViewModel, ({ viewModel }) => {
    const myFirstMethodFeature = useFeature('my.first.method.feature');
    const myFirstClassFeature = useFeature('my.first.class.feature');

    return (
        <>
            <Toggle label="my.first.method.feature" checked={myFirstMethodFeature} onText="On" offText="Off" />
            <Toggle label="my.first.class.feature" checked={myFirstClassFeature} onText="On" offText="Off" />
        </>
    );
});
