// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * The exception that is thrown as the consequence of a broken rule
 */
export class BrokenRuleError extends Error {
    constructor(readonly source: string, readonly rule: BrokenRule, message: string) {
        super(message);
    }
}

/**
 * Represents a broken rule in the system
 */
export class BrokenRule {
    constructor(readonly name: string, readonly message: string) {
    }

    /**
     * Create a broken rule
     * @param {string} name Name of the rule
     * @param {string} message Message to show - can include {argName} type of arguments for string interpolation
     * @returns {BrokenRule}
     */
    static create(name: string, message: string): BrokenRule {
        return new BrokenRule(name, message);
    }

    /**
     * Fail with the broken rule
     * @param {any | string} argsOrMessage Arguments or concrete message to fail with.
     * @param {string?} source Optional string containing the source.
     */
    fail(argsOrMessage: any | string, source = 'unknown') {
        let message = this.message;
        if (argsOrMessage) {
            if (!(argsOrMessage instanceof String)) {
                message = this.interpolateString(this.message, argsOrMessage);
            }
        }

        throw new BrokenRuleError(source, this, message);
    }

    private interpolateString(input: string, args: any): string {
        const regex = new RegExp('{(.*?)}', 'g');
        let result = input;
        const argumentKeys = Object.keys(args);

        input.match(regex)?.forEach(match => {
            const toReplace = match;
            const key = toReplace.substr(1, toReplace.length - 2);

            if (argumentKeys.some(_ => _ === key)) {
                const value = args[key];
                result = result.split(toReplace).join(value);
            }
        });

        return result;
    }
}

import { AggregateRoot } from '@dolittle/sdk.aggregates';

declare module '@dolittle/sdk.aggregates' {
    interface AggregateRoot {
        /**
         * Fail with a broken rule for the aggregate.
         * @param {BrokenRule} rule Rule to fail
         * @param {any | string} argsOrMessage Arguments or concrete message to fail with.
         */
        fail(rule: BrokenRule, argsOrMessage: any | string);
    }
}

AggregateRoot.prototype.fail = function (rule: BrokenRule, argsOrMessage: any | string) {
    rule.fail(argsOrMessage, this.constructor.name);
};


