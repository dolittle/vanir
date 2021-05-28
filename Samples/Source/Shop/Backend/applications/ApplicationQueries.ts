import { Resolver, Query, Extensions } from 'type-graphql';
import { injectable } from 'tsyringe';
import { Application } from './Application';
import { graphRoot, IMongoDatabase } from '@dolittle/vanir-backend';

@Resolver()
@graphRoot('applications')
@injectable()
export class ApplicationQueries {

    constructor(private readonly _mongoDatabase: IMongoDatabase) {
    }

    @Query(() => [Application])
    @graphRoot('something')
    async allApplications(): Promise<Application[]> {
        const collection = await this._mongoDatabase.collection(Application);
        return collection.find().toArray();
    }
}
