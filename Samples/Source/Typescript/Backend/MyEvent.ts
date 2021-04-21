import { eventType } from '@dolittle/sdk.events';


@eventType('5ff11df2-68e6-4890-8cae-b22fd1e3c8b5')
export class MyEvent {
    constructor(readonly something: number) { }
}
