// App.js
import React, {Component} from 'react';
import CurrencyPairs from '../modules/rates/components/CurrencyPairs';
import {compose} from 'lodash/fp';
import {connect} from 'react-redux';
import {hot} from 'react-hot-loader';
import {fetchCurrencyPairs} from '../modules/rates/actions';

const Loader = {
    color: "#24292e",
    textAlign: "center",
    fontSize: "35px",
    fontWeight: "bold",
    display: "flex",
    flexDirection: "column",
    fontFamily: "Helvetica",
    marginTop: "25%",
    alignItems: "stretch"
};

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
            <CurrencyPairs
                currencyPairs={this.props.currencyPairs}
            />
            :
            <div style={Loader}>Loading... <i className="fas fa-spinner fa-2x" /></div>)
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