import { Resolver, Query, Arg, Mutation } from 'type-graphql';
import { injectable, container } from 'tsyringe';
import { IMongoDatabase } from '../../../../Source/typescript/backend/mongodb/IMongoDatabase';
import { Nothing } from './Nothing';
import { DoStuff } from './DoStuff';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { IEventStore } from '@dolittle/vanir-backend/dist/dolittle';



@injectable()
@Resolver()
export class NoQueries {

    constructor(private readonly _mongoDatabase: IMongoDatabase, private readonly _eventStore: IEventStore) {

        this._mongoDatabase = container.resolve(IMongoDatabase as constructor<IMongoDatabase>);
        let i=0;
        i++;
    }

    @Query(returns => [Nothing])
    async noresults() {
        const collection = this._mongoDatabase.collection<Nothing>('nothings');
        return [];
    }

    @Mutation(() => Boolean)
    async perform(@Arg('input') input: DoStuff): Promise<boolean> {

        return true;
    }
}
