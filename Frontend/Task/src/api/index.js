import axios from 'axios';
import store from '../store'

import {
	getConfigurationSuccess,
	getRateSuccess,
	responseError,
	requestError
} from '../actions/currencyActions';

// get configuration from server
export const getConfiguration = () => {
	return axios.get('http://localhost:3000/configuration')
    .then((response) => {
			//dispatch to store the pairs.
      store.dispatch(getConfigurationSuccess(response.data.currencyPairs));
			//async update the rates every 5 seconds.
			store.dispatch(asyncUpdate(response.data.currencyPairs));
    }).catch(error => {
				if (error.response) {
					store.dispatch(responseError(error.response.status));
				} else if (error.request) {
						store.dispatch(requestError(error.request));
				}
		});
}
export const getRates = (id) => {
	//for each key get the rate.
  let ids = [];
  Object.keys(id).map(key => ids.push(key));
  return (dispatch) => {
    return axios.get('http://localhost:3000/rates', {
      params: {
          currencyPairIds: ids
      }
    }).then(response => {
			//dispatch it to the store.
      store.dispatch(getRateSuccess(response.data.rates));
      return true;
		}).catch(error => {
				if (error.response) {
					store.dispatch(responseError(error.response.status));
				} else if (error.request) {
						store.dispatch(requestError(error.request));
				}
		});
	}
};

const asyncUpdate = (id) => {
  return (dispatch) => {
    const updateRates = async () => {
      await dispatch(getRates(id));
      	//setTimeout(updateRates, 5000);
    };
    updateRates();
  }
};
