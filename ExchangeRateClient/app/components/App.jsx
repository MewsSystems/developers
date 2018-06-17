// App.js
import React, {Component} from 'react';
import CurrencyPairs from '../modules/rates/components/CurrencyPairs';
import {compose} from 'lodash/fp';
import {connect} from 'react-redux';
import {hot} from 'react-hot-loader';
import {fetchCurrencyPairs} from '../modules/rates/actions';

class App extends Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        this.props.fetchCurrencyPairs();
    }

    render() {

        return (this.props.currencyPairs
            ?
            <div>
                <CurrencyPairs
                    currencyPairs={this.props.currencyPairs}
                />
            </div>
            :
            'Loading...')
    }
}

function mapStateToTheProps(state) {
    return {
        currencyPairs: state.currencyPairs,
    };
}

export default compose(
    hot(module),
    connect(mapStateToTheProps, {
        fetchCurrencyPairs
    }),
)(App);