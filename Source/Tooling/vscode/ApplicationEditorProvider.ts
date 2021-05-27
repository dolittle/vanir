// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { VisualEditor } from './VisualEditor';
import fs from 'fs';
import path from 'path';

export type MicroserviceLayout = {
    left: number;
    top: number;
};

export type MicroserviceLayouts = { [key: string]: MicroserviceLayout };

export type ApplicationLayout = {
    microservices: MicroserviceLayouts;
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
    eventHorizons: EventHorizons;
    eventHorizonConsents: EventHorizonConsents;
    layout: MicroserviceLayout;
};

export type Application = {
    id: string;
    name: string;
    tenant: string;
    license: string;
    containerRegistry: string;
    portal: Portal;
    microservices: Microservice[];
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

    private loadLayoutFor(document: vscode.TextDocument): ApplicationLayout {
        const applicationDirectory = path.dirname(document.fileName);
        const applicationLayoutFile = path.join(applicationDirectory, 'application.layout');
        let applicationLayout: ApplicationLayout = { microservices: {} };
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


    beforeUpdateDocumentInView(document: vscode.TextDocument): string {
        const applicationDefinition = JSON.parse(document.getText()) as ApplicationDefinition;
        const applicationDirectory = path.dirname(document.fileName);
        const applicationLayout = this.loadLayoutFor(document);
        const hasLayout = Object.keys(applicationLayout.microservices).length > 0;

        const application: Application = { ...(applicationDefinition as any) };
        application.microservices = applicationDefinition.microservices.map(_ => {
            const microserviceDirectory = path.join(applicationDirectory, _);
            const microserviceFile = path.join(microserviceDirectory, 'microservice.json');
            if (fs.existsSync(microserviceFile)) {
                const json = fs.readFileSync(microserviceFile).toString();
                const microservice = JSON.parse(json) as Microservice;
                microservice.eventHorizons = {};
                microservice.eventHorizonConsents = {};

                if (applicationLayout.microservices.hasOwnProperty(microservice.id)) {
                    microservice.layout = applicationLayout.microservices[microservice.id];
                } else {
                    microservice.layout = { left: 0, top: 0 };
                }

                const directories = getDirectories(microserviceDirectory);
                for (const directory of directories) {
                    if (!microservice.eventHorizons) {
                        const eventHorizonsFile = path.join(directory, '.dolittle', 'event-horizons.json');
                        if (fs.existsSync(eventHorizonsFile)) {
                            const eventHorizonsJson = fs.readFileSync(eventHorizonsFile).toString();
                            microservice.eventHorizons = JSON.parse(eventHorizonsJson) as EventHorizons;
                        }
                    }

                    if (!microservice.eventHorizonConsents) {
                        const eventHorizonConsentsFile = path.join(directory, '.dolittle', 'event-horizon-consents.json');
                        if (fs.existsSync(eventHorizonConsentsFile)) {
                            const eventHorizonConsentsJson = fs.readFileSync(eventHorizonConsentsFile).toString();
                            microservice.eventHorizonConsents = JSON.parse(eventHorizonConsentsJson) as EventHorizonConsents;
                        }
                    }
                }

                return microservice;
            }

            return { name: 'n/a' } as Microservice;
        }).filter(_ => _.name != 'n/a');

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
                    this.outputChannel.appendLine(JSON.stringify(e.data));
                    this.outputChannel.show();
                } break;
            }
        });
    }
}
