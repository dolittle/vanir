import { IProjectionFor, ProjectionBuilder, projectionFor } from '@dolittle/projections';
import { Application } from './Application';
import { ApplicationCreated } from './ApplicationCreated';

@projectionFor(Application, '8094cfcc-eb4a-4580-bd6b-fe486f29a7d7')
export class ApplicationProjection implements IProjectionFor<Application> {
    define(projectionBuilder: ProjectionBuilder<any>): void {
        projectionBuilder
            .configureModel(_ => _.withName('applications'))
            .from(ApplicationCreated, _ => _
                .usingKeyFrom(e => e.applicationId)
                .set(p => p.name).to(ev => ev.name));
    }
}


