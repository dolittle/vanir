import { eventType } from '@dolittle/sdk.events';

@eventType('9ef8bbfa-c735-4f68-8cb0-bc2036d68219')
export class ApplicationCreated {
    constructor(readonly applicationId: string, readonly name: string) {}
}
