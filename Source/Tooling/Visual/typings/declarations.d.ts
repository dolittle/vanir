// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
interface Window {
	acquireVsCodeApi: <T = unknown>() => {
		getState: () => T;
		setState: (data: T) => void;
		postMessage: (msg: unknown) => void;
	}
}
