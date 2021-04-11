# View Models

The logic part of [MVVM](https://en.wikipedia.org/wiki/Model–view–viewmodel) is the view model.
A view model should be unaware of its consumer and just provide state and behavior a consumer
can leverage. In addition you should not bleed into a view model constructs that are clearly
a view concern, e.g. HTML artifacts or similar. Its focus should be on providing view agnostic
state and behavior. Naming plays also a role in this and can help in making the separation
good and concerns more clearer. Take the example of a button being clicked and you wanting to
hook that up with a method on the view model. In the view the verb of "clicked" has meaning,
because that was what happened - while in the view model, the functionality you are exposing
is not linked to something being clicked and should therefor be linked to the behavior you
are looking to provide; e.g. `itemSelected()` as a method name could be an example.

## Data binding

Core to [MVVM](https://en.wikipedia.org/wiki/Model–view–viewmodel) is the notion of data binding.
This means effectively consumption of state in the view and react to the state being modified
and also being able to alter the state as the consequence of user interaction in the view.

With React not having these concepts, but rather a more programmatic approach to its component
model it becomes different.

## Consuming a view model - data binding

Views and ViewModels should exist in separate files and be explicit about the extension for the
file being used. This will help you avoid leveraging view constructs within the view model.
A view should have the extension of `.tsx` while a view model `.ts`.

> Good practice in naming would be to have `<view name>` and then the view model be named
> `<view name>ViewModel`. E.g. : `Items` and `ItemsViewModel` for a component called Items.

Imagine a view model as follows:

```typescript
export class MyViewModel {
    someValue: number = 0;

    constructor() {
        setTimeout(() => this.someValue++, 1000);
    }

    doStuff() {
        // perform something - maybe talk to the server
    }
}
```

In the view you'd need to leverage the `withViewModel` function and then be ready to consume
the view model:

```tsx
import React from 'react';
import { withViewModel } from '@dolittle/vanir-react';
import { MyViewModel } from './MyViewModel';

export const MyView = withViewModel(MyViewModel, ({viewModel}) => {
    return (
        <div>
            The counter is {viewModel.someValue}

            <button onClick={viewModel.doStuff}>
        </div>
    );
});
```

The invalidation and re-rendering of the component happens as a consequence of the state change.
Behind the scenes all public properties are automatically made into observables and will effectively
then cause a re-render. The convention is that private properties with a `_` prefix will not be
observables.

This behavior is however going to be changed and be made more explicit with [this issue](https://github.com/dolittle-entropy/vanir/issues/134).
This will be a breaking change when implemented.

You'll also notice that the `button` is connected with the `doStuff` method through the `onClick` event.

## React Props

Components can also be implemented with props:

```tsx
import React from 'react';
import { withViewModel } from '@dolittle/vanir-react';
import { MyViewModel } from './MyViewModel';

export interface MyViewProps {
    something: number;
}

export const MyView = withViewModel<MyViewModel, MyViewProps>(MyViewModel, ({viewModel, props}) => {
    return (
        <div>
            <div>Something is set to {props.something}</div>
            <div>The counter is {viewModel.someValue}</div>
            <button onClick={viewModel.doStuff}>
        </div>
    );
});
```

The view model can also then get these props through the implementation of the [propsChanged](https://github.com/dolittle-entropy/vanir/blob/main/Source/react/mvvm/ICanBeNotifiedWhenPropsChanged.ts) method.
This method will be called on the initialization of the component.

## Route params

View Models can leverage routing information, such as parameters coming in - these can be typed and give you an easy
access to them:

```typescript
import { RouteInfo } from '@dolittle/vanir-react';

export interface MyRouteParams {
    something: string;
}


export class MyViewModel {

    constructor() {
    }

    attached(routeInfo: RouteInfo<MyRouteParams>) {
        console.log(routeInfo.params.something);
    }
}
```

Query parameters matching this will then be added to this object.

## Well known methods

View models have different methods that can be implemented to let the view model know when different
things occur. Some of these are related to the lifecycle.

Below is a list of well known methods, follow the link and you'll see the signature of these.
You can either implement the interface found in Vanir, or by convention just implement the method with
the correct signature.

| Method | Description |
| ------ | ----------- |
| [attached](https://github.com/dolittle-entropy/vanir/blob/main/Source/react/mvvm/ICanBeNotifiedWhenBeingAttached.ts) | When the component is attached in the DOM |
| [detached](https://github.com/dolittle-entropy/vanir/blob/main/Source/react/mvvm/ICanBeNotifiedWhenBeingDetached.ts) | When the component is detached from the DOM |
| [propsChanged](https://github.com/dolittle-entropy/vanir/blob/main/Source/react/mvvm/ICanBeNotifiedWhenPropsChanged.ts) | When the components props changed |
| [routeChanged](https://github.com/dolittle-entropy/vanir/blob/main/Source/react/mvvm/ICanBeNotifiedWhenRouteChanged.ts) | When the routing information changed |
