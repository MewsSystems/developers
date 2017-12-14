import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import {connect} from 'react-redux';
import {loadConfig} from './model/configuration/configurationActions';
import {fetchRates} from './model/rates/ratesActions';
import {selectProcessedRates} from './model/rates/ratesSelectors';
import RatesTable from './RatesTable';

const REFRESH_INTERVAL = 2000;

class App extends Component {

    constructor() {
        super();
        this.interval = null;
    }

    render() {
        console.log(this.props);

        return (
            <div className="App">
                <header className="App-header">
                    <img src={logo} className="App-logo" alt="logo" />
                    <h1 className="App-title">Welcome to Mews</h1>
                </header>
                <div className="App-content">
                    <RatesTable data={this.props.rates} />
                </div>
            </div>
        );
    }

    componentDidMount() {
        this.props.loadConfig();
    }

    componentWillReceiveProps(nextProps) {
        if (!this.props.configLoaded && nextProps.configLoaded) {
            this.interval = setInterval(this.props.fetchRates, REFRESH_INTERVAL);
        }
    }

    componentWillUnmount() {
        if (this.interval !== null) {
            clearInterval(this.interval);
        }
    }

}

App = connect(state => ({
    configLoaded: state.configuration.loaded,
    rates: selectProcessedRates(state),
}), {
    loadConfig,
    fetchRates,
})(App);

export default App;
