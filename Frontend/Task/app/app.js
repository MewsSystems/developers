import { endpoint, interval, server } from './config';
import React, {Component} from 'react'; //je potřeba aby fungovala 'jsx' syntaxe
import ReactDOM from 'react-dom'; //vstupní bod pro renderování react
import AppLayout from './components/layout';


export function run(element) {
    console.log('App is running.');
    ReactDOM.render(<AppLayout/>, document.getElementById('exchange-rate-client')); 
} 

