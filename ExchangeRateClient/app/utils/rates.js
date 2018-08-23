// @flow
import axios from 'axios';
import config from 'Config/app-config.json';

// Fetch Currency Pairs
const FetchCurrencyPairs = (): Promise<void> => {
  const url = config.pairsEndpoint;

  const axiosConfig = {
    headers: {
      'Content-Type': 'application/json;charset=UTF-8',
    },
  };

  return new Promise(resolve => {
    axios
      .get(url, axiosConfig)
      .then(res => resolve(res.data))
      .catch(err => {
        // eslint-disable-next-line
        console.error('AXIOS ERROR: ', err);
      });
  });
};

const FetchCurrencyRates = (params): Promise<void> => {
  const url = config.ratesEndpoint;

  const axiosConfig = {
    headers: {
      'Content-Type': 'application/json;charset=UTF-8',
    },
    params,
  };

  return new Promise(resolve => {
    axios
      .get(url, axiosConfig)
      .then(res => resolve(res.data))
      .catch(err => {
        // eslint-disable-next-line
        console.error('AXIOS ERROR: ', err);
      });
  });
};

export { FetchCurrencyPairs, FetchCurrencyRates };
