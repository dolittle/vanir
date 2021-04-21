import React, { useState } from 'react';
import { PrimaryButton, TextField, DetailsList, IColumn } from '@fluentui/react';
import { withViewModel } from '@dolittle/vanir-react';
import { AppViewModel } from './AppViewModel';

export const App = withViewModel(AppViewModel, ({ viewModel }) => {
    const [name, setName] = useState('');
    const columns: IColumn[] = [
        { name: "Name", key: "name", fieldName: "name", minWidth: 100 }
    ];


    return (
        <>
            <TextField label="Application name" onChange={(ev, nv) => setName(nv!)} />
            <PrimaryButton onClick={() => viewModel.createApplication(name)}>Click me for magic</PrimaryButton>

            <DetailsList columns={columns} items={viewModel.applications}>
            </DetailsList>
        </>
    );
});
