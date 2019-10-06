import React, { Component } from 'react';
import CurrencyPair from './components/currencyPair/currencyPair';
import './App.css';

export default class App extends Component {

    componentDidMount() {
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        
    }

    render() {
        return (
                <div className="App">
                    <CurrencyPair/>
                </div>
                )
    }
}