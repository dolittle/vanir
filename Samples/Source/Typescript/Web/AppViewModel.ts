import { DataSource } from '@dolittle/vanir-web';
import { injectable } from 'tsyringe';
import gql from 'graphql-tag';
import { Guid } from '@dolittle/rudiments';


@injectable()
export class AppViewModel {
    applications: any[] = [];

    constructor(private readonly _dataSource: DataSource) {
        this.populate();
    }


    async populate() {
        const query = gql`
            query {
                allApplications {
                    name
                }
            }`;
        const result = await this._dataSource.query({ query });

        /*const observableQuery = await this._dataSource.watchQuery({ query });
        observableQuery.startPolling(1000);
        observableQuery.subscribe((result) => {*/
        this.applications = result.data.allApplications;
        //})
    }


    async createApplication(name: string): Promise<void> {
        const guid = Guid.create();
        const mutation = gql`
            mutation {
                createApplication(command:{
                    applicationId:"${guid}",
                    name:"${name}"
                })
            }`;

        await this._dataSource.mutate({
            mutation
        });
    }
}
