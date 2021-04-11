# Navigator

When leveraging the MVVM model offered by Vanir, you still need to be able to navigate at times
from a [view model](view-models.md). This is achieved by taking a dependency to something called
`INavigator`.

```typescript
import { INavigator } from '@dolittle/vanir-web';
import { injectable } from 'tsyringe';

@injectable()
export class MyViewModel {
    constructor(private readonly _navigator: INavigator) {
    }

    navigateToSomething() {
        this._navigator.navigateTo('/something');
    }
}
```

What this does is to publish a message to the [messenger](../messenger.md) that can be consumed
called `NavigatedTo`. So if you have a composed with with multiple components that has their own
[view model](view-models.md), you can react to to the message actively:

```typescript
import { IMessenger, NavigatedTo } from '@dolittle/vanir-web';
import { injectable } from 'tsyringe';

@injectable()
export class MyOtherViewModel {
    constructor(private readonly _messenger_: IMessenger) {
        _messenger.subscribeTo(NavigatedTo, (message) => {
            console.log(message.path);
        });
    }
}
```

The navigation is caught by the router in the general [routing](./routing.md) and will cause it to
react to the change in navigation if it has any routes configured that matches what is being
navigated to.

## Microservice within a composition

One of the things that happens in a composition is that the message being published will bubble up
to the composition and make sure to update the top level routing and ultimately the address bar
reflecting the deep linkable route within the application and the concrete microservice route within.
