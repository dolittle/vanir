# Microservice Instance

Dolittle provides a hosted [platform](https://dolittle.io/docs/platform/) for running microservices.
Vanir represents a way to create microservices that fit with this. In addition to the requirements from
the core Dolittle platform, the Vanir project is somewhat opinionated on [architecture](./architecture.md),
packaging and how they are configured to run.

The reasoning behind this is to provide a consistent model that should boost productivity for developers
working with it due to familiarity. With a consistent model it becomes easier to create tools to go with it
that again increases productivity. It is also easier to create cross cutting solutions and avoid repetition
in implementation.

## Agnostic to implementation details

With everything being docker images, it is really just the contract to the outside world that is important.
Choices made in the [architecture](./architecture.md) with GraphQL being the contract to the frontend and
any ReST API surface being the contract to the outside world, it does not really matter how it is implemented.

## URL scheme

There are 2 concerns for any URLs in a Vanir based microservice; private and public.
The private URLs are used for the frontend [composition](./frontend/composition.md), including the
GraphQL and the second is for public APIs.

### Frontend

All microservices in the composition are expected to be on the route `/_/<microservice>`. GraphQL
endpoint is expected on `/_/<microservice>/graphql`.

### Public API

All public APIs are expected at the route `/api/<microservice>`. The exception to this is the microservice designated as the portal, it only has `/api` as the prefix.

## HTTP headers

In front of a Dolittle microservice hosted in the Dolittle platform, sits a cross cutting system for dealing
with authentication and tenancy for the application. This system adds the following HTTP headers that can
be leveraged in the frontend and the backend.


| Name | Description |
| ----- | ----------- |
| User-ID | The unique identifier of the user logging in |
| Tenant-Id | The unique identifier of the tenant the user is logged into |
| Cookie | The authentication cookie used |


## Packaging

All microservices are packaged as Docker images. The goal is to package them as optimal as possible for both
size and startup time.

The images are expected to contain the backend and the frontend if the microservice has a frontend.
This means that the backend must serve the frontend as well as any APIs and/or GraphQL endpoints

## Environment variables

Once deployed and running in the Dolittle platform, there are environment variables that will be set that can
be used by the backend:


| Name | Description |
| ----- | ----------- |
| PORT | The public port. Typically when run in the Dolittle platform, this is set to 80 |
| DOLITTLE__RUNTIME__PORT | The private port that the Dolittle SDK will use to connect to the Dolittle Runtime |
| DATABASE__HOST | The host that holds the database (MongoDB) |
| DATABASE__NAME | The name of the database that holds collections (MongoDB) |
| DATABASE__PORT | The database port (default 27017) |
| EVENTSTORE__HOST | The eventstore database host |
| EVENTSTORE__NAME | The eventstore database name (MongoDB) |
| EVENTSTORE__PORT | The eventstore database port (default 27017) |


> The current approach for database and eventstore will change in the future with a multi-tenanted approach.
> The eventstore variables are only used today by the projection engine, which is today embedded in the backend but will later be part of the runtime.
> That means that the DATABASE and EVENTSTORE environment variables will either go away or change.
