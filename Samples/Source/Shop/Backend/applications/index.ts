import { ApplicationCommandHandlers } from './ApplicationCommandHandlers';
import { ApplicationCreated } from './ApplicationCreated';
import { ApplicationEventHandler } from './ApplicationEventHandler';
import { ApplicationProjection } from './ApplicationProjection';
import { ApplicationStatisticsProjection } from './ApplicationStatisticsProjection';
import { ApplicationQueries } from './ApplicationQueries';

export const EventTypes = [
    ApplicationCreated
];

export const CommandHandlers = [
    ApplicationCommandHandlers
];

export const Queries = [
    ApplicationQueries
];

export const EventHandlers = [
    ApplicationEventHandler
];

export const Projections = [
    ApplicationProjection,
    ApplicationStatisticsProjection
];
