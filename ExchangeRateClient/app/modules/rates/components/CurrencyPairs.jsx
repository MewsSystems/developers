import React, {Component} from 'react';
import {compose} from "lodash/fp";
import {connect} from "react-redux";
import {hot} from "react-hot-loader";
import {fetchCurrencyRates} from "../actions";

class CurrencyPairs extends Component {
    constructor(props) {
        super(props);

        this.state = {
            timer: null
        }
    }

    componentDidMount() {
        let arrayOfIds = [];
        this.props.currencyPairs.forEach(x => arrayOfIds.push(x['id']));

        let timer = setInterval(() => this.props.fetchCurrencyRates(arrayOfIds), 15000);
        this.setState({timer});
    }

    componentWillUnmount() {
        this.clearInterval(this.state.timer);
    }

    render() {
        const {currencyPairs} = this.props;

        const currencyLis = currencyPairs.map(currency => (
            <li key={currency.id}>{currency.currency1['code'] + ' / ' + currency.currency2['code']}</li>
        ));

        let newArr = [];

        for (let key in this.props.rates) {
            if (this.props.rates.hasOwnProperty(key)) {
                let newObj = {
                    id: key,
                    rate: this.props.rates[key]
                };
                newArr.push(newObj);
            }
        }

        return (
            <div>
                <ul>
                    {currencyLis}
                </ul>
                <p>yolo</p>
                <ul>
                    {newArr.map(currency => (
                        <li key={currency.id}>{currency.rate}</li>
                    ))}
                </ul>
            </div>
        );
    }
};

// export default CurrencyPairs;

function mapStateToTheProps(state) {
    return {
        rates: state.rates,
    };
}

export default compose(
    hot(module),
    connect(mapStateToTheProps, {
        fetchCurrencyRates
    }),
)(CurrencyPairs);