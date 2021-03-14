# Anatomy of a backend

This document describes the anatomy of a backend.

## Package.json

Within the `Backend` folder there should be a [`package.json`](./package-json.md) file that describes the backend.

### Scripts

Within the `package.json` it is good practice to have the shorthand scripts for being able to run things.
It is also preferable to provide scripts that provide a watch type of workflow. Part of the scripts could be also to provide
scripts for runnings tests (specs - BDD) and to perform linting on the code.
In addition, there should be a way for the Continuous Integration pipelines to build the code.

For a backend, this could typically look like below:

```json
{
    "scripts": {
        "dev": "nodemon --inspect=0 -e ts --exec node -r ts-node/register index.ts",
        "start": "ts-node index.ts",
        "clean": "tsc -b --clean",
        "build": "yarn clean && webpack --mode=production",
        "test": "mocha",
        "lint": "eslint '**/*.{js,ts,tsx}' --quiet --fix",
        "lint:ci": "eslint '**/*.{js,ts,tsx}' --quiet",
        "ci": "yarn clean && yarn lint:ci && tsc -b && yarn test"
    }
}
```

> Notice there are 2 different lint scripts. One for the CI and one for local. The point of this is that the local one
> is set up to perform any fixes. We don't want the CI pipeline to perform these and hide any issues, since it doesn't
> commit it back.

### Nodemon

As you can see from the scripts above, its using something called [nodemon](https://nodemon.io).
The purpose of this tool is to provide a watch type of workflow, whenever code changes - it automatically restarts and
performs any tasks you want it to perform before it all starts up again.

The configuration itself can sit directly on the `package.json` file. Below is a sample of this that
configures it to watch a couple of folders for changes, ignore the `dist` folder and any declaration files.
In the scripts section, notice the usage of it with `nodemon ./index.ts`.

```json
{
    "nodemonConfig": {
        "restartable": "rs",
        "ignore": [
            "dist",
            "*.d.ts"
        ],
        "execMap": {
            "ts": "ts-node"
        },
        "watch": [
            "./",
            "../../Shared/Backend",
            "../../Shared/DependencyInversion"
        ],
        "ext": "ts"
    }
}
```

## Entrypoint

Your entrypoint, typically `index.ts` needs to hook up with Vanir using the `Host` object.
You can read more about that [here](./host.md).
