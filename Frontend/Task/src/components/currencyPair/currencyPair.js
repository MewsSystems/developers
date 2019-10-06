import React, { Component } from 'react';
import { endpoint, interval } from './../../assets/config';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
export default class CurrencyPair extends Component {

    CONF_URL = "http://localhost:3000/configuration";
    rates = {};
    constructor(props) {
        super(props);
        setInterval(() => this.refresh(), interval);


    }

    componentDidMount() {
        this.setState({
            loaded: false,
            ratesNew: [""],
            ratesOld: [""]
        });
        this.loadCurrencies();
    }

    refresh() {
        if (this.state.loaded) {
            this.loadPrices();
        }
    }

    loadCurrencies() {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", this.CONF_URL, true);
        xhr.onload = function (e) {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    var json_obj = JSON.parse(xhr.responseText);
                    this.setState({
                        loaded: true,
                        currencies: json_obj.currencyPairs
                    });
                    this.loadPrices();
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

    paramArrayBuilder(name, items) {
        if (items.length > 0) {
            let result = "?" + name + items[0];
            for (var i = 0; i < items.length; i++) {
                result += "&" + name + "=" + items[i];
            }
            return result;
        }
        return "";
    }

    loadPrices() {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", endpoint + this.paramArrayBuilder("currencyPairIds", Object.keys(this.state.currencies)), true);
        xhr.onload = function (e) {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    var json_obj = JSON.parse(xhr.responseText);
                    this.setState({
                        loadPricesStrike: 0
                    });
                    this.setState({
                        ratesNew: json_obj.rates
                    });
                    const rates = json_obj.rates;
                    for (const [key, value] of Object.entries(rates)) {

                        if (this.rates != null && this.rates[key] != null) {
                            if (parseFloat(this.rates[key]["value"]) < parseFloat(value)) {
                                this.rates[key] = {
                                    value: value,
                                    trend: true,
                                    display: this.rates[key]["display"]
                                };
                            } else if (parseFloat(this.rates[key]["value"]) > parseFloat(value)) {
                                this.rates[key] = {
                                    value: value,
                                    trend: false,
                                    display: this.rates[key]["display"]
                                };
                            } else {
                                this.rates[key] = {
                                    value: value,
                                    trend: null,
                                    display: this.rates[key]["display"]
                                };
                            }
                        } else {
                            this.rates[key] = {
                                value: value,
                                trend: null,
                                display: Math.random() >= 0.5
                            };
                        }
                    }
                } else {
                    this.setState({
                        loadPricesStrike: this.state.loadPricesStrike + 1
                    });
                    console.error(xhr.statusText);
                    if (this.state.loadPricesStrike < 4) {
                        this.loadPrices();
                    } else {
                        console.error("Server is probably down. Repeating every " + interval / 1000 + " seconds.");
                    }
                }
            }
        }.bind(this);
        xhr.onerror = function (e) {
            console.error(xhr.statusText);
        };
        xhr.send(null);
    }
    
    prepareRates() {
        const rates = [];
        for (const [key, value] of Object.entries(this.rates)) {
            if(this.rates[key]["display"] === true) {
                const tmp = {
                    key: key,
                    name: this.state.currencies[key][0]["code"] + "/" + this.state.currencies[key][1]["code"],
                    value: this.rates[key]["value"],
                    trend: this.rates[key]["trend"]
                }
                rates.push(tmp);
            }
        }
        return rates;
    }
    
    render() {
        const rates = this.prepareRates();
        return (
                <BootstrapTable data={rates} striped>
                    <TableHeaderColumn isKey dataField='name'></TableHeaderColumn>
                    <TableHeaderColumn dataField='value'>Value</TableHeaderColumn>
                </BootstrapTable>
                )
    }
}