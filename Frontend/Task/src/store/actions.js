import { Proxy } from 'proxies';

export default {
  getConfiguration: () => {
    return (dispatch) => new Promise((resolve, reject) => {
      const configuration = localStorage.getItem('configuration');
      if (configuration) {
        dispatch({ type: 'CONFIG_SUCCESS', data: JSON.parse(configuration) });
        resolve();
      }
      else {
        dispatch({ type: 'CONFIG_REQUEST' });
        new Proxy().getData({ url: 'configuration' }).then(response => {
          localStorage.setItem('configuration', JSON.stringify(response.currencyPairs));
          dispatch({ type: 'CONFIG_SUCCESS', data: response.currencyPairs });
          resolve();
        }).catch(error => {
          dispatch({ type: 'CONFIG_ERROR', error });
          reject();
        });
      }
    });
  },
  getRates: () => {
    return (dispatch, getState) => new Promise((resolve, reject) => {
      const currencyPairIds = Object.keys(getState().configuration.data);
      dispatch({ type: 'RATES_REQUEST' });
      new Proxy({ currencyPairIds }).getData({ url: 'rates' }).then(response => {
        dispatch({ type: 'RATES_SUCCESS', data: response.rates });
        resolve(response);
      }).catch(error => {
        console.log(error)
        dispatch({ type: 'RATES_ERROR', error });
        reject();
      });
    });
  },
  updateFilter: (data) => {
    return dispatch => new Promise((resolve, reject) => {
      dispatch({ type: 'UPDATE_FILTER', data });
      localStorage.setItem('filter', JSON.stringify(data));
      resolve();
    });
  }
};