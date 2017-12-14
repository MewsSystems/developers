import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import {connect} from 'react-redux';
import {loadConfig} from './model/configuration/configurationActions';
import {fetchRates} from './model/rates/ratesActions';
import {selectProcessedRates} from './model/rates/ratesSelectors';
import {setFilter} from './model/filter/filterActions';
import RatesTable from './RatesTable';
import Filter from './Filter';

const REFRESH_INTERVAL = 2000;

class App extends Component {

    constructor() {
        super();
        this.interval = null;
    }

    onFilterChange = event => {
        this.props.setFilter(event.target.value);
    }

    render() {
        return (
            <div className="App">
                <header className="App-header">
                    <img src={logo} className="App-logo" alt="logo" />
                    <h1 className="App-title">Welcome to Mews</h1>
                </header>
                <div className="App-content">
                    <Filter value={this.props.filter} onChange={this.onFilterChange} />
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
    filter: state.filter,
    configLoaded: state.configuration.loaded,
    rates: selectProcessedRates(state),
}), {
    loadConfig,
    fetchRates,
    setFilter,
})(App);

export default App;
