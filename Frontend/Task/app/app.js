import React from 'react';
import ReactDOM from 'react-dom';
import ExchangeRateApp from './src/ExchangeRateApp'

export function run(element) {
    ReactDOM.render(<ExchangeRateApp/>, document.getElementById(element));
}