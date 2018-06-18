import React, {Component} from 'react';
import {compose} from "lodash/fp";
import {connect} from "react-redux";
import {hot} from "react-hot-loader";
import {fetchCurrencyRates} from "../actions";

class CurrencyPairs extends Component {
    constructor(props) {
        super(props);

        this.state = {
            timer: null,
            anotherTimer: null,
            currencyPairs: {},
            propsArray: []
        };

    }

    componentDidMount() {
        this.props.fetchCurrencyRates(Object.keys(this.props.currencyPairs));
        let timer = setInterval(() => this.props.fetchCurrencyRates(Object.keys(this.props.currencyPairs)), 15000);
        let anotherTimer = setInterval(() => this.setState({currencyPairs: this.props.currencyPairs}), 15000);
        this.setState({
            timer,
            anotherTimer
        });
    }

    componentWillUnmount() {
        this.clearInterval(this.state.timer);
        this.clearInterval(this.state.anotherTimer);
    }

    propsToArray() {
        let newArr = [];
        const {currencyPairs} = this.props;
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
        this.setState({
            propsArray: newArr
        });
    }

    render() {
        const {currencyPairs} = this.props;

        console.log(this.state);

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
            <li key={currency.id}>{currency.currency1['code'] + ' / ' + currency.currency2['code'] + ' = ' + (currency.rate ? currency.rate['rate'] : '') + ' ' + (currency.prevRate ? (currency.rate['rate'] > currency.prevRate['rate']) ? 'up' : 'down' : 'same')}</li>
        ));

        return (
            <div>
                <ul>
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