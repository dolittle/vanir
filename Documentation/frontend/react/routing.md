# Routing

There are basically two levels of routing going on in a [composition](../composition.md):

1. Top level / portal - the composition level
2. Within a microservice

Both have an impact on how the user and developer experience is.

## Routing sub system goals

The following goals have gone into the design of this sub system:

1. Enable composition
2. Make composition transparent for the end user
3. Make composition transparent for the developer

## General

All routing is done built on top of the [React Router](https://reactrouter.com).
The `<Bootstrapper/>` component as described in the [getting started guide](./getting-started.md), adds
the `<BrowserRouter/>` component and renders all children given to the bootstrapper within this.
In addition it adds something called `<RouteNavigator/>` that enables it to respond to route changes
happening from anything using the [navigator](../navigator.md).

If you add your own `<BrowserRouter/>` to any of your component, it is then vital that you add the
`<RouteNavigator/>` within it to guarantee that it will respond to any route changes done through
the [navigator](../navigator.md).

```tsx
import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';

export const MyComponent = () => {

    return (
        <Router>
            <RouteNavigator/>
        </Router>
    )

};
```

## Composition level

On the composition level one represents the top level browser window and document. This means
that the address bar should from a user perspective reflect a natural path within the application
and not be tainted with the technical details of the composition of microservices.

However, you want enough information from the route itself to know which microservice to show
content for. In Vanir, this is solved by convention.

Since the `<Bootstrapper/>` has the router already set up, we can simply start using the components
of the [React Router](https://reactrouter.com) straight out of the box.

```tsx
import React from 'react';
import { Switch, Route } from 'react-router-dom';

export const App = () => {
    return (
        <Bootstrapper name="MyComposition" prefix="" version="1.0.0">
            <Switch>
                <Route exact path="/">
                    <h1>This is the home</h1>
                </Route>
            </Switch>
        </Bootstrapper>
    )
}
```

For any microservices you want to include in your composition there is a special kind of **route** component
called `<CompositionRoute/>`. This follows the convention and includes an `iframe` within it that is then setup
with the convention in mind.

Example:

```tsx
import React from 'react';
import { Switch, Route } from 'react-router-dom';

export const App = () => {
    return (
        <Bootstrapper name="MyComposition" prefix="" version="1.0.0">
            <Switch>
                <Route exact path="/">
                    <h1>This is the home</h1>
                </Route>
                <CompositionRoute path="/my-microservice"/>
            </Switch>
        </Bootstrapper>
    )
}
```

With this, the `iframe` within will automatically have its URL prefixed with the prefix configured, but from
a user perspective it will be accessible through the path setup that the route from a browser perspective is
available at. Everything past this in the URL will be carried over to the actual microservice within the `iframe`.

## Within a Microservice

For microservices that are within the composition, they should not take on the responsibility of the composition and
should in fact be blissfully unaware of that. The `<Bootstrapper/>` configuration should be the only awareness of this.
They do however need to configure their "routing table" in a different way to make the convention work as intended,
leveraging components found in Vanir for this rather than out-of-the-box components from [React Router](https://reactrouter.com).

There are two components that will enable you to do routing within your microservice; `<Routing/>` and `<MicroserviceRoute/>`.

Example:

```tsx
import React from 'react';
import { Routing, MicroserviceRoute } from '@dolittle/vanir-react';

export const App = () => {
    return (
        <Bootstrapper name="MyMicroservice" prefix="/_/" version="1.0.0">
            <Routing>
                <MicroserviceRoute
                    exact
                    path="/">
                    <h1>This is the home - within the microservice</h1>
                </MicroserviceRoute>
            </Routing>
        </Bootstrapper>
    )
}
```

The `<MicroserviceRoute/>` component is a wrapper around the [`<Route/>`](https://reactrouter.com/web/api/Route) component
and its `props` are in fact the same as the original component and is passed along. The `path` property is however
modified before passed along - as that is the thing that needs to be set up for the convention to work and the actual
route within the `iframe` in which the router responds to.

## Read more

If you're curious to how it fits together, you should read more about the [microservice context](./microservice-context.md).
