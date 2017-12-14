import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import {connect} from 'react-redux';
import {loadConfig} from './model/configuration/configurationActions';
import {fetchRates} from './model/rates/ratesActions';

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
                    <h1 className="App-title">Welcome to React</h1>
                </header>
                <p className="App-intro">
                </p>
            </div>
        );
    }

    componentDidMount() {
        this.props.loadConfig();
    }

    componentWillReceiveProps(nextProps) {
        if (!this.props.configuration.loaded && nextProps.configuration.loaded) {
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
    configuration: state.configuration,
    rates: state.rates.data,
}), {
    loadConfig,
    fetchRates,
})(App);

export default App;
