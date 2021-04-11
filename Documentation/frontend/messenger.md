# Messenger

When creating decoupled frontends that consists of a composition of views/components,
you need a way be able to communicate changes without coupling the different components
together and creating a maintenance nightmare. The [messenger pattern in MVVM](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/mvvm-messenger-and-view-services-in-mvvm) is a good one.

A good example of this could be a master/detail type of scenario where you have a list of items and when
you click an item you want to show the details of this. These can be implemented as two different components
with their own respective view models behind them.

The messenger in Vanir is built around the messages being known types and uses this as the 'topic' when
publishing. This message becomes the known artifact; the contract that everyone can rely on as the carrier
of information without coupling the two components to each other.

Lets imaging we have the following message type in a file called `ItemSelected.ts`:

```typescript
export class ItemSelected {
    constructor(readonly id: string) {}
}
```

In the list view model you'd do something like this:

```typescript
import { IMessenger } from '@dolittle/vanir-web';
import { injectable } from 'tsyringe';
import { ItemSelected } from './ItemSelected';

@injectable()
export class ListViewModel {
    constructor(private readonly _messenger_: IMessenger) {
    }


    itemSelected(id: string) {
        _messenger.publish(new ItemSelected(id));
    }
}
```

In the details view model you can now subscribe to this whenever and from whomever it is published from:

```typescript
import { IMessenger, NavigatedTo } from '@dolittle/vanir-web';
import { injectable } from 'tsyringe';
import { ItemSelected } from './ItemSelected';

@injectable()
export class DetailsViewModel {
    details: any;

    constructor(private readonly _messenger_: IMessenger) {
        _messenger.subscribeTo(ItemSelected, async (message) => {
            const response = await fetch(`/api/items/details/${message.id}`);

            // ... error handling + possible logic/transformation ...

            this.details = response.json();
        });
    }
}
```

## Observables

The messenger is built on top of [RxJS](https://rxjs.dev). Instead of using the `subscribeTo()` approach, you
could leverage the `observe()` method or even `observeAll()`. These gives you observables that enables you
to create your own more complex pipelines for reacting to messages.

## Composition

Since the messenger is hooked up in the general composition, you can effectively use it as a communication vessel
between toe top level composition and any microservices within.
