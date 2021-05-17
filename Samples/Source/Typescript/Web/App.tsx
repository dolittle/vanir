// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { useState } from 'react';
import { withViewModel } from '@dolittle/vanir-react';
import { AppViewModel } from './AppViewModel';
import { useFeature } from '@dolittle/vanir-react';

export const App = withViewModel(AppViewModel, ({ viewModel }) => {
    const myFirstFeatureMethodFeature = useFeature('my.first.method.feature');

    return (
        <>
        </>
    );
});
