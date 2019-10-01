import React, { Component } from 'react';
import { endpoint, interval } from './config';
import CurrencyPairRateList from './components/currencyPairRateList/currencyPairRateList';
import './App.css';

export default class App extends Component {

    CONF_URL = "http://localhost:3000/configuration";
    status = false

    constructor(props) {
        super(props);
        this.reporter();


    }

    reporter() {
        console.log(endpoint);
        console.log(interval);
    }

    componentDidMount() {
        this.loadCurrencies();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        this.render();
    }

    getStatus() {
        return this.status ? "true" : "false";
    }

    loadCurrencies() {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", this.CONF_URL, true);
        xhr.onload = function (e) {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    var json_obj = JSON.parse(xhr.responseText);
                    this.setState(this.status = true);
                    console.log(json_obj);
                    this.render();
                } else {
                    console.error(xhr.statusText);
                }
            }
        }.bind(this);
        xhr.onerror = function (e) {
            console.error(xhr.statusText);
        };
        xhr.send(null);
    }

    render() {
        return (
                <div className="App">
                    <h1>{this.getStatus()}</h1>
                    <CurrencyPairRateList/>
                </div>
                )
    }
}