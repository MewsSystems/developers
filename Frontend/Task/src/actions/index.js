/* eslint-disable import/prefer-default-export */
import Axios from 'axios';

export const getConfiguration = () => dispatch => {
  console.log('run-getConfiguration');
  Axios.get('http://localhost:3000/configuration', { timeout: 10000 })
    .then(response => {
      const configuration = response.data.currencyPairs;
      console.log('RUN action', configuration);
      return dispatch({
        type: 'CONFIGURATION',
        payload: configuration,
      });
    })
    .catch(error => console.log('getConfiguration error', error));
};

export const test = {
  type: 'TEST',
  payload: 'test-success',
};
