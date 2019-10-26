import { endpoint, interval } from './config.json';
import { render } from 'react-dom';
import { Main } from './Main';
import * as React from 'react';

export function run(element: HTMLElement) {
    render(React.createElement(Main), element);
}
