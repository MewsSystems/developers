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

    const rates = configurationRates.map(key => {
      const request = {
        params: {
          currencyPairIds: [key[0]],
        },
      };

      const rateColl = [];

      Axios.get('http://localhost:3000/rates', request, {
        timeout: 10000,
      })
        .then(response => {
          rateColl.push(Object.entries(response.data.rates));
        })
        .catch(error => console.log('error', error));

      return rateColl;
    });

    console.log('RATESARRAY', rates);

    this.setState({
      rates,
    });
  }

  render() {
    const { configuration, rates } = this.state;

    console.log('run3: render', rates);
    const currencyCouples = configuration.map((key, i) => {
      // const rate = rates[i] ? rates[i][0][0][1] : '';
      console.log('RATE', rates[i]);
      return (
        <li className="list-group-item" key={key[0]}>
          <p>element: {i}</p>
          <p>code1: {key[1][0].code}</p>
          <p>name1: {key[1][0].name}</p>
          <p>code2: {key[1][1].code}</p>
          <p>name2: {key[1][1].name}</p>
          <br />
          <p>rates: {rates[i]}</p>
        </li>
      );
    });

    return <ul className="list-group">{currencyCouples}</ul>;
  }
}
