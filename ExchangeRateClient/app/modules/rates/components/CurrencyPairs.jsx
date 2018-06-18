import React, {Component} from 'react';
import {compose} from "lodash/fp";
import {connect} from "react-redux";
import {hot} from "react-hot-loader";
import {fetchCurrencyRates} from "../actions";

const Common = {
    fontFamily: "Helvetica",
    color: "#24292e",
    boxSizing: "border-box",
    width: "50%",
    margin: "0 auto"
};

const Header = {
    padding: "10px 20px",
    textAlign: "center",
    fontSize: "28px",
    fontWeight: "bold",
    marginTop: "10%"
};

const List = {
    textAlign: "center",
    listStyle: "none",
    padding: "0"
};

const Li = {
    lineHeight: "2"
};

class CurrencyPairs extends Component {
    constructor(props) {
        super(props);

        this.state = {
            timer: null,
            anotherTimer: null,
            currencyPairs: {}
        };

    }

    componentDidMount() {
        this.props.fetchCurrencyRates(Object.keys(this.props.currencyPairs));
        let timer = setInterval(() => this.props.fetchCurrencyRates(Object.keys(this.props.currencyPairs)), 10000);
        let anotherTimer = setInterval(() => this.setState({currencyPairs: this.props.currencyPairs}), 10000);
        this.setState({
            timer,
            anotherTimer
        });
    }

    componentWillUnmount() {
        this.clearInterval(this.state.timer);
        this.clearInterval(this.state.anotherTimer);
    }

    render() {
        const {currencyPairs} = this.props;

        let newArr = [];
        for (let key in currencyPairs) {
            if (currencyPairs.hasOwnProperty(key)) {
                let newObj = {
                    id: key,
                    currency1: currencyPairs[key][0],
                    currency2: currencyPairs[key][1],
                    rate: currencyPairs[key][2],
                    prevRate: (this.state.currencyPairs[key] ? this.state.currencyPairs[key][2] : undefined)
                };
                newArr.push(newObj);
            }
        }

        const currencyLis = newArr.map(currency => (
            <li key={currency.id} style={Li}>
                {currency.currency1['code'] + ' / ' + currency.currency2['code'] + ' = ' + (currency.rate ? (currency.rate['rate']) : '...') + ' '}
                <i className={"fas fa-long-arrow-alt-" + (currency.prevRate ? (currency.rate['rate'] > currency.prevRate['rate']) ? 'up' : 'down' : '')} />
            </li>
        ));

        return (
            <div style={Common}>
                <header style={Header}>Exchange Rates</header>
                <ul style={List}>
                    {currencyLis}
                </ul>
            </div>
        );
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
        fetchCurrencyRates
    }),
)(CurrencyPairs);