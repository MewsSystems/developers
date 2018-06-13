// App.js
import React, { Component } from 'react'
import { compose } from 'lodash/fp';
import { connect } from 'react-redux';
import { hot } from 'react-hot-loader'
import { fetchCurrencyPairs } from '../modules/rates/actions';

class App extends Component {
    componentDidMount() {
        this.props.fetchCurrencyPairs();
    }

    render() {
        return (
            <div>Hello, World!</div>
        );
    }
}

export default compose(
    hot(module),
    connect(undefined, {
        fetchCurrencyPairs,
    }),
)(App);
