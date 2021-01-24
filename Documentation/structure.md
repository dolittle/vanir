# Structure

The tooling offered by Vanir is somewhat opinionated on structure. It assumes the following:

```shell
project
|   application.json
└───Source
    └───<microservice>
        |   microservice.json
        └───Backend
        |   |   Dockerfile
        └───Web (optional)
```

Both the creation of microservices tooling and the build pipelines are built for this structure.
