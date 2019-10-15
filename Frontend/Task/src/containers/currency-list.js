import React, { Component } from 'react';
import Axios from 'axios';

export default class CurrencyList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      configuration: [],
      rates: [],
    };
    this.getConfiguration();
  }

  getConfiguration() {
    console.log('run1');
    Axios.get('http://localhost:3000/configuration', { timeout: 10000 }).then(
      response => {
        const configuration = Object.entries(response.data.currencyPairs);
        console.log('RUN', configuration);
        this.setState({
          configuration,
        });
        this.getRates(configuration);
      }
    );
  }

  getRates(configurationRates) {
    console.log('run2', configurationRates[0][0]);

    configurationRates.forEach(key => {
      const request = {
        params: {
          currencyPairIds: [key[0]],
        },
      };

      // const call = async () => {
      //   const res = await Axios.get('http://localhost:3000/rates', request, {
      //     timeout: 10000,
      //   });
      //   const rate = res.data.rates;
      //   console.log('rate', rate);
      //   const R1 = Object.entries(res.data.rates);
      //   const R2 = Object.keys(res.data.rates);
      //   this.setState({ rates: [...this.state.rates, [R1, R2]] });
      // };

      // call();

      Axios.get('http://localhost:3000/rates', request, {
        timeout: 10000,
      })
        .then(response => {
          const rate = Object.entries(response.data.rates);
          // const R2 = Object.keys(response.data.rates);
          this.setState(state => {
            const rates = [...state.rates, rate];
            return {
              rates,
            };
          });
        })
        .catch(error => console.log('error', error));
    });
  }

  render() {
    const { configuration, rates } = this.state;

    console.log('run3 - C', configuration);
    // console.log('run3 - R', rates);
    // console.log('run3 - C', configuration instanceof Array);
    const currencyCouples = configuration.map(key => {
      // const strollo = rates[i] ? rates[i][0][0][1] : 'n/c';
      let rate = [];

      if (rates) {
        console.log('YES');
        rates.filter(j => {
          if (j[0][0] === key[0]) {
            console.log('RATE-x', j);
            rate = j[0][1];
          }
        });
      }

      // rates.filter(j => j[0][0] === key[0]);

      // if (rates[any][0][0] === key[0]) {return rates[0][0][1]};
      // forEach
      console.log('KEY-C', key[0]);
      console.log('KEY-R', rates);
      return (
        <li className="list-group-item" key={key[0]}>
          <p>element: {key[0]}</p>
          <p>code1: {key[1][0].code}</p>
          <p>name1: {key[1][0].name}</p>
          <p>code2: {key[1][1].code}</p>
          <p>name2: {key[1][1].name}</p>
          <br />
          <p>rates: {rate.length !== 0 ? rate : 'n/c'}</p>
        </li>
      );
    });

    return <ul className="list-group">{currencyCouples}</ul>;
  }
}
