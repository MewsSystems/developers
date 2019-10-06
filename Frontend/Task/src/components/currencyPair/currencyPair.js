import React, { Component } from 'react';
import { endpoint, interval, maxErrorStrike } from './../../assets/config';
import { message } from './../../assets/info';
import RateList from './RateList/rateList';
import Selector from './Selector/selector';
import { Spinner, Container, Row, Col, Toast } from 'react-bootstrap'
import Cookies from 'universal-cookie';
import './styles.css';

export default class CurrencyPair extends Component {

    CONF_URL = "http://localhost:3000/configuration";
    rates = {};
    loaded = false;
    crash = false;
    loadedCurrencies = false;
    constructor(props) {
        super(props);
        setInterval(() => this.refresh(), interval);


    }

    componentDidMount() {
        this.loadCurrencies();
    }

    refresh() {
        this.loadPrices();
    }

    loadCurrencies() {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", this.CONF_URL, true);
        xhr.onload = function (e) {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    var json_obj = JSON.parse(xhr.responseText);
                    this.loadedCurrencies = true;
                    this.setState({
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
        if (this.loadedCurrencies) {
            var xhr = new XMLHttpRequest();
            xhr.open("GET", endpoint + this.paramArrayBuilder("currencyPairIds", Object.keys(this.state.currencies)), true);
            xhr.onload = function (e) {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        var json_obj = JSON.parse(xhr.responseText);
                        this.setState({
                            loadPricesStrike: 0
                        });
                        this.crash = false;
                        this.setState({
                            ratesNew: json_obj.rates
                        });
                        const rates = json_obj.rates;
                        for (const [key, value] of Object.entries(rates)) {

                            if (this.rates != null && this.rates[key] != null) {
                                const cookies = new Cookies();
                                if(cookies.get(key) != null) {
                                    this.rates[key]["display"] = (cookies.get(key) === 'true');
                                }
                                if (parseFloat(this.rates[key]["value"]) < parseFloat(value)) {
                                    this.rates[key] = {
                                        value: value,
                                        trend: '↑ ',
                                        display: this.rates[key]["display"]
                                    };
                                } else if (parseFloat(this.rates[key]["value"]) > parseFloat(value)) {
                                    this.rates[key] = {
                                        value: value,
                                        trend: '↓ ',
                                        display: this.rates[key]["display"]
                                    };
                                } else {
                                    this.rates[key] = {
                                        value: value,
                                        trend: '= ',
                                        display: this.rates[key]["display"]
                                    };
                                }
                                cookies.set(key, this.rates[key]["display"], { path: '/' });
                            } else {
                                this.rates[key] = {
                                    value: value,
                                    trend: null,
                                    display: true
                                };
                            }
                        }
                        this.forceUpdate();
                        this.loaded = true;
                    } else if(xhr.status >= 500) {
                        this.setState({
                            loadPricesStrike: this.state.loadPricesStrike + 1
                        });
                        console.error(xhr.statusText);
                        if (this.state.loadPricesStrike < maxErrorStrike) {
                            this.loadPrices();
                        } else {
                            this.crash = true;
                            console.error("Server is not responding. Repeating every " + interval / 1000 + " seconds.");
                            this.forceUpdate();
                        }
                    } 
                }
            }.bind(this);
            xhr.onerror = function (e) {
                console.error(xhr.statusText);
            };
            xhr.send(null);
        }
    }
    
    selectorCallback = (currency) => {
        this.rates[currency["currencyId"]]["display"] = currency["value"];
        const cookies = new Cookies();
        cookies.set(currency["currencyId"], currency["value"], { path: '/' });
        this.forceUpdate();
    }
    
    render() {
        const loaded = this.loaded;
        var crash = this.crash;
        const toggleHide = () => { this.crash = false; this.forceUpdate(); }; 
        var currencies = [];
        if(this.state != null && this.state.currencies != null) {
            currencies = this.state.currencies;
        }
        return (
                <div>
                    <Loader loaded={loaded}/>
                    <Container>
                        <Row>
                        <Toast show={crash} onClose={toggleHide}>
                            <Toast.Header>
                                <img src="data:image/gif;base64,R0lGODlhAQABAIAAAAUEBAAAACwAAAAAAQABAAACAkQBADs=" className="rounded mr-2" alt="" />
                                <strong className="mr-auto">Server failure</strong>
                            </Toast.Header>
                            <Toast.Body>Server is not responding. Repeating every {interval / 1000}</Toast.Body>
                        </Toast>
                        <Col><p>{message}</p></Col>
                        </Row>
                        <Row>
                            <Col >
                                <Selector rates={this.rates} currencyList={this.selectorCallback} currencies={currencies}/>
                            </Col >
                            <Col >
                                <RateList rates={this.rates} currencies={currencies}/>
                            </Col >
                        </Row>
                    </Container>
                </div>
                )
    }
}

export class Loader extends Component {

        render() {
            if (!this.props.loaded) {
                return (
                        <div className='loader'>
                            <Spinner animation="border" role="status">
                                <span className="sr-only">Loading...</span>
                            </Spinner>  
                        </div>
                        )
            } else {
                return(
                        <div className='loader hidden'>
                            <Spinner animation="border" role="status">
                                <span className="sr-only">Loading...</span>
                            </Spinner>  
                        </div>
                    ) 
            }
        }
}