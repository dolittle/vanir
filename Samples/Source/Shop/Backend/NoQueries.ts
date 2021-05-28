// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Resolver, Query, Arg, Mutation } from 'type-graphql';
import { injectable, container } from 'tsyringe';
import { Nothing } from './Nothing';
import { DoStuff } from './DoStuff';
import { guid, IAggregate, IEventStore, IMongoDatabase } from '@dolittle/vanir-backend';
import { AggregateRoot, aggregateRoot, on } from '@dolittle/sdk.aggregates';
import { Client } from '@dolittle/sdk';
import { MyEvent } from './MyEvent';
import { Guid } from '@dolittle/rudiments';

import { projectionFor, IProjectionFor, ProjectionBuilder } from '@dolittle/projections';
import { feature } from '@dolittle/vanir-features';

export class CustomError extends Error {
    constructor() {
        super('Something went wrong');
    }
}

@aggregateRoot('619d0ab1-a831-48da-a247-673e7799a398')
export class MyAggregate extends AggregateRoot {
    doStuff(something: number): void {
        //throw new Error('Something is screwed');
        this.apply(new MyEvent(something));
        this.applyPublic(new MyEvent(something));
    }

    @on(MyEvent)
    onMyEvent(event: MyEvent) {
        let i = 0;
        i++;
    }
}

export class SomeModel {
    @guid()
    _id!: Guid;

    something!: number;
}

@projectionFor(SomeModel,'646231fa-716e-497b-9118-95bfaec81558')
export class MyProjection implements IProjectionFor<SomeModel> {
    define(projectionBuilder: ProjectionBuilder<SomeModel>): void {
        projectionBuilder
            .configureModel(_ => _.withName('models'))
            .from(MyEvent, _ => _
                .set(p => p.something).to(e => e.something))
    }
}



@injectable()
@Resolver()
export class NoQueries {

    constructor(private readonly _mongoDatabase: IMongoDatabase, private readonly _eventStore: IEventStore, private readonly _aggregate: IAggregate, private readonly _client: Client) {
        var i = 0;
        i++;
    }

    @Query(returns => [Nothing])
    async noresults() {
        const collection = this._mongoDatabase.collection<Nothing>('nothings');
        return [];
    }

    @Mutation(() => Boolean)
    async perform(@Arg('input') command: DoStuff): Promise<boolean> {
        const aggregate = await this._aggregate.of(MyAggregate, command.id);
        await aggregate.perform(_ => _.doStuff(command.something));
        return true;
    }
}
