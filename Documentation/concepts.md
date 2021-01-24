# Concepts

This page explains the concepts found in Vanir.

## Application

A Vanir application is the logical grouping of microservices composed together to bring
the end user a feeling of a single application.

From the templates you get an `application.json` files when creating a new application.
In this it holds a relative path to any microservices.

## Microservice

A microservice is an autonomous deployable unit. Any elements within a microservice is versioned
together and deployed together. Anything within it is considered cohesive and should be
modelled as such.

From the template you get a `microservice.json` file that contains details about the microservice
in the folder of the microservice.

## Monorepo

Vanir has been optimized for the concept of a monorepo. This means that it is expecting all
microservices to be in the same repository - or on disk in the same containing folder.
(Look [here](./structure.md) for more details on structure).

A monorepo does not need to be physically a monorepo from a source control perspective.
One way to get around this is to leverage Git sub modules and symbolic linking to adhere to
the expected structure.

## Versioning

Every Microservice is expected to be versioned independently from one another. By default this is
achieved by the build pipelines working with the `microservice.json` file coming from the templates.

The pipelines are built to update three properties of the file:

```json
{
    "version": "1.0.0",
    "commit": "c35ee1bb075b26d7fb9d0481332eb5bc578cedfc",
    "built": "2021-01-23T18:09:29.374Z"
}
```

## Portal

One of the concepts in Vanir is something called Portal. This is the outermost microservice that
is responsible for the [composition](./frontend/composition.md) of itself together with all the
microservices in the composition.
