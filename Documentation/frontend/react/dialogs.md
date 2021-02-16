# Dialogs

Most applications want to show pop up dialogs, either with messages or with more complex configuration.
Having to put the dialog code, both the view and any logic in the same place as the host of the dialog
tends to pollute and making unmaintainable code.

Separating these out is easy with the use of the `useDialog()` hook in Vanir.

Lets say you have a dialog leveraging [FluentUIs Dialog](https://developer.microsoft.com/en-us/fluentui#/controls/web/dialog):
This can then be formalized into its own component like below:

```tsx
const dialogContentProps: IDialogContentProps = {
    type: DialogType.normal,
    title: 'My Dialog',
    closeButtonAriaLabel: 'Close'
};

export const MyDialog = () => {
    return (
        <Dialog
            minWidth={600}
            onDismiss={done}
            dialogContentProps={dialogContentProps}>

            ...
            Some cool content
            ...

            <DialogFooter>
                <PrimaryButton text="Done" />
                <DefaultButton text="Cancel" />
            </DialogFooter>
        </Dialog>
    );
}
```

With the `useDialog()` hook we get a way to interact with the dialog:

- Send input into it.
- Setup callback thats get called when the dialog is closed.
- Get the output provided from the close - including dialog result.
- The ability to show the dialog when needed.

Lets say we have a host view that will show this dialog on a button click. We can do the following:

```tsx
import { MyDialog } from './MyDialog';
import { DialogResult, useDialog } from '@dolittle/vanir-react';

export const MyHostComponent = () => {
    const [showMyDialog, myDialogProps] = useDialog(async (result, output?) => {
        if (result === DialogResult.Success) {
            // Great, we have success
        } else {
            // Bummer
        }
    });

    return (
        <>
            <PrimaryButton onClick={showMyDialog}>Show the dialog</PrimaryButton>
            <MyDialog {...myDialogProps}/>
        </>
    )
};
```

What we then need to do is change the dialog component to take the dialog props from Vanir and also react on the property for showing
the dialog:

```tsx
import { Dialog, DialogFooter, PrimaryButton, DefaultButton } from '@fluentui/react';
import { DialogResult, IDialogProps } from '@dolittle/vanir-react';

const dialogContentProps: IDialogContentProps = {
    type: DialogType.normal,
    title: 'My Dialog',
    closeButtonAriaLabel: 'Close'
};

export const MyDialog = (props: IDialogProps) => {

    // Done method that gets called when the done button is clicked
    const done = () => {
        props.onClose(DialogResult.Success, { /* the result object */ })
    };

    // Cancel method that gets called when the cancel button is clicked
    const cancel = () => {
        props.onClose(DialogResult.Cancelled, { /* the result object */ })
    };

    return (
        <Dialog
            minWidth={600}
            hidden={!props.visible}     // We react on the visible property from the props
            onDismiss={done}
            dialogContentProps={dialogContentProps}>

            ...
            Some cool content
            ...

            <DialogFooter>
                <PrimaryButton onClick={done} text="Done" />
                <DefaultButton onClick={cancel} text="Cancel" />
            </DialogFooter>
        </Dialog>
    );
}
```

## Input & Output

The dialog can have typed input and output. You specify this by leveraging the generic parameters of `useDialog()`.
Introduce your type declaration for input and outputs:

```tsx
export interface MyDialogInput {
    something: string;
}

export interface MyDialogOutput {
    result: number;
}
```

You can then leverage this in the dialog directly:

```tsx
import { Dialog, DialogFooter, PrimaryButton, DefaultButton } from '@fluentui/react';
import { DialogResult, IDialogProps } from '@dolittle/vanir-react';

import { MyDialogInput } from './MyDialogInput';
import { MyDialogOutput } from './MyDialogOutput';

const dialogContentProps: IDialogContentProps = {
    type: DialogType.normal,
    title: 'My Dialog',
    closeButtonAriaLabel: 'Close'
};

export const MyDialog = (props: IDialogProps<MyDialogInput, MyDialogOutput>) => {

    // Done method that gets called when the done button is clicked
    const done = () => {
        const output: MyDialogOutput = {
            result: 42;
        };
        props.onClose(DialogResult.Success, output)
    };

    // Cancel method that gets called when the cancel button is clicked
    const cancel = () => {

        // Since we're cancelling, we don't want to have any output
        props.onClose(DialogResult.Cancelled)
    };

    return (
        <Dialog
            minWidth={600}
            hidden={!props.visible}     // We react on the visible property from the props
            onDismiss={done}
            dialogContentProps={dialogContentProps}>

            ...
            Some cool content
            ...

            <DialogFooter>
                <PrimaryButton onClick={done} text="Done" />
                <DefaultButton onClick={cancel} text="Cancel" />
            </DialogFooter>
        </Dialog>
    );
}
```

In the host view, you get the following:

```tsx
import { MyDialog } from './MyDialog';
import { MyDialogInput } from './MyDialogInput';
import { MyDialogOutput } from './MyDialogOutput';
import { DialogResult, useDialog } from '@dolittle/vanir-react';

export const MyHostComponent = () => {
    const [showMyDialog, myDialogProps] = useDialog<MyDialogInput, MyDialogOutput>(async (result, output?) => {
        if (result === DialogResult.Success) {
            // Great, we have typed output in our success
            console.log(output.result);
        } else {
            // Bummer
        }
    });

    return (
        <>
            <PrimaryButton onClick={showMyDialog}>Show the dialog</PrimaryButton>
            <MyDialog {...myDialogProps}/>
        </>
    )
};
```
