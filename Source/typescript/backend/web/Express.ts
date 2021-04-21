// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import express, { Express } from 'express';
import { ApolloServer } from 'apollo-server-express';
import compression from 'compression';
import morgan from 'morgan';
import bodyParser from 'body-parser';
import path from 'path';
import { Configuration } from '../Configuration';
import { logger } from '../logging';
import { getSchemaFor } from '../graphql';
import swaggerUI from 'swagger-ui-express';
import { ContextMiddleware, getCurrentContext } from '../Context';
import { BackendArguments } from '../BackendArguments';

export let app: Express;
export type ExpressConfigCallback = (app: Express) => void;

export async function initialize(configuration: Configuration, backendArguments: BackendArguments) {
    const prefix = `${configuration.isRooted ? '' : '/_'}/${configuration.routeSegment}`;

    if (prefix.length > 0) {
        logger.info(`Using '${prefix}' as prefix`);
    } else {
        logger.info('Using no prefix');
    }

    app = express();
    app.use(ContextMiddleware);

    app.use(morgan(':method :url :status :res[content-length] - :response-time ms') as any);
    app.use(compression());
    app.use(
        bodyParser.urlencoded({
            extended: true
        })
    );
    app.use(bodyParser.json());

    const server = new ApolloServer({
        schema: await getSchemaFor(configuration, backendArguments),
        context: () => {
            return getCurrentContext();
        }
    });
    const graphqlRoute = `${prefix}/graphql`.replace('//', '/');

    logger.info(`Hosting graphql at ${graphqlRoute}`);

    server.applyMiddleware({ app, path: graphqlRoute });

    if (backendArguments.swaggerDoc) {
        let swaggerPath = '/api';
        if (configuration.routeSegment.length > 0) {
            swaggerPath = `${swaggerPath}/${configuration.routeSegment}`;
        }
        swaggerPath = `${swaggerPath}/swagger`;
        logger.info(`Hosting swagger at '${swaggerPath}'`);
        app.use(
            swaggerPath,
            swaggerUI.serve,
            swaggerUI.setup(backendArguments.swaggerDoc, {}, {})
        );
    }

    backendArguments.expressCallback?.(app);

    logger.info(`Serving static content from '${configuration.publicPath}'`);

    app.use(prefix, express.static(configuration.publicPath));
    app.use((req, res) => {
        const indexPath = path.resolve(configuration.publicPath, 'index.html');
        res.sendFile(indexPath);
    });

    const expressPort = process.env.PORT || configuration.port;

    logger.info(`Express will be listening to port '${expressPort}'`);
    app.listen({ port: expressPort, hostname: '0.0.0.0' }, () => {
        logger.info(`Server is now running on http://localhost:${expressPort}`);
    });

    return app;
}
