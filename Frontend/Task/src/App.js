import React from 'react';
import './App.css';
import './bootstrap.css'
import axios from 'axios';
import update from 'immutability-helper';

class App extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            currencies: {},
            state_ids_codes_list: {},
            state_complete_codes_list: [],
            cur_id: {},
            currency_rates_list: {},
            actual_current_list: {},
            state_ids_rates_json_initial: {},
            currencyPairState: "Loading...",
            rateState: "Loading...",
            trendState: "Loading..."
        }
    }

    componentDidMount() {
        this.getCurrencies();
        setInterval(() => this.getInitialRateList(), 5000);
    }

    getCurrencies = () => {
        axios.get('http://localhost:3000/configuration')
            .then(res => {
                const state = {};
                for (let entry of Object.entries(res.data.currencyPairs)) {
                    const [key, value] = entry;
                    state[key] = {code: value[0].code + "/" + value[1].code, rate: 'Loading...', trend: 'Loading...'}
                }
                this.setState({currencies: state});
                this.getInitialRateList();
            })
            .catch(err => alert(err.response.data))
    };

    compare = (now, before) => {
        if (before === 'Loading...') return 'stagnating';
        if (before === now)  return 'stagnating';
        if (now > before) return 'growing';
        if (before > now) return 'declining';
    };

    getInitialRateList = () => {
        Object.entries(this.state.currencies).map(([key, value]) => (
            axios.get('http://localhost:3000/rates', {params: {currencyPairIds: JSON.stringify([key])}})
                .then(res => {
                    const new_currencies = update(this.state.currencies, {[key]: {$set:
                        {
                            code: this.state.currencies[key].code,
                            rate: res.data.rates[key],
                            trend: this.compare(res.data.rates[key], this.state.currencies[key].rate)
                        }
                    }});
                    this.setState({currencies: new_currencies});
                })
                .catch(err => {
                    console.log(err.response ? err.response.data:err)
                })
        ));
    };

    selectColor = status => {
        switch (status) {
            case 'stagnating':
                return 'orange';
            case 'growing':
                return 'green';
            case 'declining':
                return 'red';
            default:
                return null
        }
    };

    renderSomething() {
        return (
            <div class="container">
                <div class="row justify-content-center">
                    <div style={{background: 'WhiteSmoke'}} class="col-6">
                        <h1 class="text-center">Currency Selector</h1>

                        <table class="table" id="currencyPairName">
                            <thead>
                            <tr>
                                <th scope="col">Currencies Pairs</th>
                                <th scope="col">Rate</th>
                                <th scope="col">Trend</th>
                            </tr>
                            </thead>
                            <tbody>
                            {Object.entries(this.state.currencies).map(([id, currency]) => (
                                <tr>
                                    <td>{currency.code}</td>
                                    <td id="rate-in-table">{currency.rate}</td>
                                    <td style={{color: this.selectColor(currency.trend)}}>{currency.trend}</td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        )
    }

    renderLoading() {
        return (
            <p>loading...</p>
        )
    }

    render() {
        return Object.keys(this.state.currencies).length ? this.renderSomething():this.renderLoading()

    }

}

export default App;