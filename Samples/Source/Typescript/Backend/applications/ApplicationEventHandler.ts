import { EventContext } from '@dolittle/sdk.events';
import { eventHandler, handles } from '@dolittle/sdk.events.handling';
import { IMongoDatabase, IEventStore } from '@dolittle/vanir-backend';
import { ApplicationCreated } from './ApplicationCreated';
import { Application } from './Application';
import { Guid } from '@dolittle/rudiments';
import { injectable } from 'tsyringe';

@eventHandler('3ab5b650-0c6f-4606-8fd6-9386334b576a')
@injectable()
export class ApplicationEventHandler {

    constructor(private readonly _eventStore: IEventStore, private readonly _mongoDatabase: IMongoDatabase) {
        let i = 0;
        i++;
    }
    //, private readonly _mongoDatabase: IMongoDatabase


    @handles(ApplicationCreated)
    async applicationCreated(event: ApplicationCreated, eventContext: EventContext): Promise<void> {
        //this._eventStore.commitPublic(event, eventContext.eventSourceId);

        let i = 0;
        i++;

        /*
        const collection = await this._mongoDatabase.collection(Application);
        const application = new Application();
        application._id = Guid.parse(event.applicationId);
        application.name = event.name;
        collection.insertOne(application);*/
    }
}
