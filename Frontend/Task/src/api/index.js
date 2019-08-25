import axios from 'axios';
import store from '../store'

import {
 getConfigurationSuccess,
} from '../actions/currencyActions';

export function getConfiguration() {
	return axios.get('http://localhost:3000/configuration')
    .then((response) => {
      store.dispatch(getConfigurationSuccess(response.data));
    });
}
