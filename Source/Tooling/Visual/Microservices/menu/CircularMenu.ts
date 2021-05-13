// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable react/no-find-dom-node */

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import * as d3 from 'd3';
import * as PieTypes from './PieTypes';
import { PieText } from './PieText';
import { Pie } from './Pie';
import { RenderResult, SvgInstance } from './SvgInstance';
import { CircularMenuConfig } from './CircularMenuConfig';
import { ItemClicked, ItemMouseOut, ItemMouseOver } from './CallbackTypes';

export const DEFAULT_CONFIG = {
    type: PieTypes.HALF,
    colors: [],
    width: null,
    showIcon: false,
    sizeIcon: '2em',
    innerRadius: 50,
    showCenteredLabel: true,
    centeredLabelFontSize: '1.5em',
    backgroundColor: 'rgba(255, 255, 255, 0)'
};

export interface CircularMenuProps {
    style: any,
    data: any[];
    config: CircularMenuConfig;
    show: boolean;
    // innerRadius: PropTypes.number,
    onItemClick?: ItemClicked,
    onItemMouseOver?: ItemMouseOver,
    onItemMouseOut?: ItemMouseOut
}

// white transparent

export class CircularMenu extends Component<CircularMenuProps> {
    private svgInstance!: SvgInstance;
    private pie!: Pie;
    private text!: PieText;

    constructor(props: CircularMenuProps) {
        super(props);
    }

    componentDidMount() {
        const _props = this.props;
        const data = _props.data;
        const config = _props.config;

        const configuration = { ...DEFAULT_CONFIG, ...this.props.config };
        // init svg, pie, centered text
        this.svgInstance = new SvgInstance(ReactDOM.findDOMNode(this) as Element, configuration);
        this.pie = new Pie(this.svgInstance);
        this.text = new PieText(this.svgInstance);
        // render pie if data
        if (data) {
            this.svgInstance.render(data, config);
        }
        // Namespaced resize event
        const ns = new Date().valueOf();
        d3.select(window).on('resize.' + ns, () => {
            if (this.props.show) {
                return this.redraw(this.props);
            }
        });
    }


    UNSAFE_componentWillReceiveProps(nextProps: CircularMenuProps) {
        if (nextProps.show) {
            this.redraw(nextProps);
        }
    }

    onItemClick(d) {
        this.props.onItemClick?.(d);
    };

    onItemMouseOver(d) {
        const configuration = { ...DEFAULT_CONFIG, ...this.props.config };
        if (configuration.showCenteredLabel) {
            this.text.render(d.label, configuration.type, configuration.centeredLabelFontSize);
            this.props.onItemMouseOver?.(d);
        }
    }

    /**
     * Click on segment / item
     */

    onItemMouseOut(d) {
        this.text.hide();
        this.props.onItemMouseOut?.(d);
    }

    /**
     * Redraw elements
     */

    redraw(props: CircularMenuProps) {
        const data = props.data;
        const config = props.config;

        const configuration = { ...DEFAULT_CONFIG, ...config };

        if (data) {
            // render svg
            this.svgInstance.render(data, configuration);
            // render pie
            this.pie.render(data, configuration, (d) => {
                return this.onItemClick(d);
            }, (d) => {
                return this.onItemMouseOver(d);
            }, (d) => {
                return this.onItemMouseOut(d);
            });
        }
    };

    render() {
        return React.createElement('div', { styles: this.props.style });
    };
}
