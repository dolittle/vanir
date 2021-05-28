// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { VisualEditor } from './VisualEditor';
import fs from 'fs';
import path from 'path';
import { TenantId } from '@dolittle/sdk.execution';
import { Guid } from '@dolittle/rudiments';

export type MicroserviceLayout = {
    left: number;
    top: number;
};

export type EventHorizonLayout = {
    producer: string;
    consumer: string;
    producerHandle: string;
    consumerHandle: string;
};

export type MicroserviceLayouts = { [key: string]: MicroserviceLayout };

export type ApplicationLayout = {
    microservices: MicroserviceLayouts;
    eventHorizonLayouts: EventHorizonLayout[];
};

export type Portal = {
    enabled: boolean;
    id: string;
};

export type EventHorizon = {
    microservice: string;
    tenant: string;
    stream: string;
    partition: string;
    scope: string;
};

export type EventHorizons = { [key: string]: EventHorizon[] };

export type EventHorizonConsent = {
    microservice: string;
    tenant: string;
    stream: string;
    partition: string;
    consent: string;
};

export type EventHorizonConsents = { [key: string]: EventHorizonConsent[] };

export type Microservice = {
    id: string;
    name: string;
    version: string;
    commit: string;
    built: string;
    web: boolean,
    root: string;
    eventHorizons: EventHorizons;
    eventHorizonConsents: EventHorizonConsents;
    eventHorizonsFile: string;
    eventHorizonConsentsFile: string;
    layout: MicroserviceLayout;
};

export type Application = {
    id: string;
    name: string;
    tenant: string;
    license: string;
    containerRegistry: string;
    portal: Portal;
    root: string;
    tenants: string[];
    microservices: Microservice[];
    layout: ApplicationLayout;
};

export type ApplicationDefinition = {
    id: string;
    name: string;
    tenant: string;
    license: string;
    containerRegistry: string;
    portal: Portal;
    microservices: string[];
};

const getDirectories = source =>
    fs.readdirSync(source, { withFileTypes: true })
        .filter(dirent => dirent.isDirectory())
        .map(dirent => path.join(source, dirent.name));

export class ApplicationEditorProvider extends VisualEditor {
    private static readonly viewType = 'dolittle.application';

    public static register(context: vscode.ExtensionContext): vscode.Disposable {
        const provider = new ApplicationEditorProvider(context);
        const providerRegistration = vscode.window.registerCustomEditorProvider(ApplicationEditorProvider.viewType, provider);
        return providerRegistration;
    }

    constructor(context: vscode.ExtensionContext) {
        super(context);
    }

    get name(): string {
        return 'application';
    }

    beforeUpdateDocumentInView(document: vscode.TextDocument): string {
        const applicationLayout = this.loadLayoutFor(document);
        const hasLayout = Object.keys(applicationLayout.microservices).length > 0;

        const application: Application = this.loadApplication(document, applicationLayout);

        if (!hasLayout) {
            application.microservices.forEach((microservice, index) => {
                microservice.layout = {
                    left: ((index % 4) * 250),
                    top: ((Math.floor(index / 4)) * 100)
                };
                microservice.layout.left += 20;
                microservice.layout.top += 20;

                applicationLayout.microservices[microservice.id] = microservice.layout;
            });

            this.saveLayoutFor(document, applicationLayout);
        }

        return JSON.stringify(application);
    }



    async onInitialize(
        document: vscode.TextDocument,
        webviewPanel: vscode.WebviewPanel,
        token: vscode.CancellationToken
    ): Promise<void> {
        webviewPanel.webview.onDidReceiveMessage(e => {
            switch (e.type) {
                case 'microserviceMoved': {
                    const layout = this.loadLayoutFor(document);
                    if (layout.microservices.hasOwnProperty(e.data.id)) {
                        layout.microservices[e.data.id].left = e.data.left;
                        layout.microservices[e.data.id].top = e.data.top;
                    }

                    this.saveLayoutFor(document, layout);
                } break;

                case 'connect': {
                    vscode.window.showInformationMessage('Connecting the dots');
                    const layout = this.loadLayoutFor(document);
                    const application = this.loadApplication(document, layout);

                    const producer = application.microservices.find(_ => _.id.toLowerCase() == e.data.producer.toLowerCase());
                    const consumer = application.microservices.find(_ => _.id.toLowerCase() == e.data.consumer.toLowerCase());

                    if (producer && consumer) {
                        this.setupConsentsForAllTenants(application, producer, consumer);
                        this.setupEventHorizonsForAllTenants(application, producer, consumer);

                        this.saveEventHorizonConsents(producer);
                        this.saveEventHorizons(consumer);

                        layout.eventHorizonLayouts = layout.eventHorizonLayouts || [];

                        layout.eventHorizonLayouts.push({
                            producer: producer.id,
                            consumer: consumer.id,
                            producerHandle: e.data.producerHandle,
                            consumerHandle: e.data.consumerHandle
                        });

                        this.saveLayoutFor(document, layout);
                    }

                } break;
            }
        });
    }

    private setupConsentsForAllTenants(application: Application, producer: Microservice, consumer: Microservice) {
        for (const tenant of application.tenants) {
            const consents: EventHorizonConsent[] = [];
            if (producer.eventHorizonConsents.hasOwnProperty(tenant)) {
                producer.eventHorizonConsents[tenant].forEach(_ => consents.push(_));
            }

            const microserviceId = consumer.id;
            const streamId = producer.id;
            const partitionId = Guid.empty.toString();

            const consent: EventHorizonConsent = {
                microservice: microserviceId,
                tenant: tenant,
                stream: streamId,
                partition: partitionId,
                consent: Guid.create().toString()
            };

            if (consents.some(_ =>
                _.microservice === consent.microservice &&
                _.tenant === consent.tenant &&
                _.stream === consent.stream &&
                _.partition === consent.partition)) {
                continue;
            }

            consents.push(consent);
            producer.eventHorizonConsents[tenant] = consents;
        }
    }

    private setupEventHorizonsForAllTenants(application: Application, producer: Microservice, consumer: Microservice) {
        for (const tenant of application.tenants) {
            const eventHorizon: EventHorizon[] = [];
            if (consumer.eventHorizons.hasOwnProperty(tenant)) {
                consumer.eventHorizons[tenant].forEach(_ => eventHorizon.push(_));
            }

            const microserviceId = producer.id;
            const streamId = microserviceId;
            const partitionId = Guid.empty.toString();
            const scopeId = streamId;

            const consent: EventHorizon = {
                microservice: microserviceId,
                tenant: tenant,
                stream: streamId,
                partition: partitionId,
                scope: scopeId
            };

            if (eventHorizon.some(_ =>
                _.microservice === consent.microservice &&
                _.tenant === consent.tenant &&
                _.stream === consent.stream &&
                _.partition === consent.partition &&
                _.scope === consent.scope)) {
                continue;
            }

            eventHorizon.push(consent);
            consumer.eventHorizons[tenant] = eventHorizon;
        }
    }

    private saveEventHorizonConsents(microservice: Microservice) {
        const json = JSON.stringify(microservice.eventHorizonConsents, null, 2);
        fs.writeFileSync(microservice.eventHorizonConsentsFile, json);
    }

    private saveEventHorizons(microservice: Microservice) {
        const json = JSON.stringify(microservice.eventHorizons, null, 2);
        fs.writeFileSync(microservice.eventHorizonsFile, json);
    }

    private loadApplication(document: vscode.TextDocument, applicationLayout: ApplicationLayout) {
        const applicationDefinition = JSON.parse(document.getText()) as ApplicationDefinition;
        const applicationDirectory = path.dirname(document.fileName);

        const application: Application = { ...(applicationDefinition as any) };
        application.root = applicationDirectory;
        application.layout = applicationLayout;

        const tenantsFile = path.join(applicationDirectory, '.dolittle', 'tenants');
        if (fs.existsSync(tenantsFile)) {
            const json = fs.readFileSync(tenantsFile).toString();
            application.tenants = Object.keys(JSON.parse(json));
        } else {
            application.tenants = [TenantId.development.toString()];
        }

        application.microservices = applicationDefinition.microservices.map(_ => {
            const microserviceDirectory = path.join(applicationDirectory, _);
            const microserviceFile = path.join(microserviceDirectory, 'microservice.json');

            if (fs.existsSync(microserviceFile)) {
                const json = fs.readFileSync(microserviceFile).toString();
                const microservice = JSON.parse(json) as Microservice;
                microservice.eventHorizons = {};
                microservice.eventHorizonConsents = {};
                microservice.root = microserviceDirectory;

                if (applicationLayout.microservices.hasOwnProperty(microservice.id)) {
                    microservice.layout = applicationLayout.microservices[microservice.id];
                } else {
                    microservice.layout = { left: 0, top: 0 };
                }

                const directories = getDirectories(microserviceDirectory);
                for (const directory of directories) {
                    const eventHorizonsFile = path.join(directory, '.dolittle', 'event-horizons.json');
                    if (fs.existsSync(eventHorizonsFile)) {
                        const eventHorizonsJson = fs.readFileSync(eventHorizonsFile).toString();
                        microservice.eventHorizons = JSON.parse(eventHorizonsJson) as EventHorizons;
                        microservice.eventHorizonsFile = eventHorizonsFile;
                    }

                    const eventHorizonConsentsFile = path.join(directory, '.dolittle', 'event-horizon-consents.json');
                    if (fs.existsSync(eventHorizonConsentsFile)) {
                        const eventHorizonConsentsJson = fs.readFileSync(eventHorizonConsentsFile).toString();
                        microservice.eventHorizonConsents = JSON.parse(eventHorizonConsentsJson) as EventHorizonConsents;
                        microservice.eventHorizonConsentsFile = eventHorizonConsentsFile;
                    }
                }

                return microservice;
            }

            return { name: 'n/a' } as Microservice;
        }).filter(_ => _.name != 'n/a');
        return application;
    }

    private loadLayoutFor(document: vscode.TextDocument): ApplicationLayout {
        const applicationDirectory = path.dirname(document.fileName);
        const applicationLayoutFile = path.join(applicationDirectory, 'application.layout');
        let applicationLayout: ApplicationLayout = { microservices: {}, eventHorizonLayouts: [] };
        if (fs.existsSync(applicationLayoutFile)) {
            const json = fs.readFileSync(applicationLayoutFile).toString();
            applicationLayout = JSON.parse(json) as ApplicationLayout;
        }
        return applicationLayout;
    }

    private saveLayoutFor(document: vscode.TextDocument, applicationLayout: ApplicationLayout) {
        const applicationDirectory = path.dirname(document.fileName);
        const applicationLayoutFile = path.join(applicationDirectory, 'application.layout');
        const json = JSON.stringify(applicationLayout, null, 2);
        fs.writeFileSync(applicationLayoutFile, json);
    }
}
