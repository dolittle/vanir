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
import { SubscriptionServer } from 'subscriptions-transport-ws';
import { execute, subscribe } from 'graphql';
import { createServer } from 'http';

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

    const graphqlRoute = `${prefix}/graphql`.replace('//', '/');
    const schema = await getSchemaFor(configuration, backendArguments);
    const apolloServer = new ApolloServer({
        schema,
        subscriptions: {
            path: graphqlRoute
        },
        context: () => {
            return getCurrentContext();
        }
    });

    logger.info(`Hosting graphql at ${graphqlRoute}`);
    backendArguments.expressCallback?.(app);

    apolloServer.applyMiddleware({ app, path: graphqlRoute });

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


    logger.info(`Serving static content from '${configuration.publicPath}'`);

    app.use(prefix, express.static(configuration.publicPath));
    app.use((req, res) => {
        const indexPath = path.resolve(configuration.publicPath, 'index.html');
        res.sendFile(indexPath);
    });

    const expressPort = process.env.PORT || configuration.port;

    const server = createServer(app);

    logger.info(`Express will be listening to port '${expressPort}'`);
    server.listen({ port: expressPort, hostname: '0.0.0.0' }, () => {
        new SubscriptionServer({
            execute,
            subscribe,
            schema,
        }, {
            server,
            path: graphqlRoute
        });

        logger.info(`Server is now running on http://localhost:${expressPort}`);
    });

    return app;
}
