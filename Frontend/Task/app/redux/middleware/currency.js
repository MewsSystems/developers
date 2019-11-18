import { LOAD_CURRENCY_CONFIGURATION, SET_CURRENCY_CONFIGURATION } from '../constants';
import api from '../../api/connectors';

export const currencyMiddleware = (store) => (next) => (action) => {
  next(action);

  switch (action.type) {
    case LOAD_CURRENCY_CONFIGURATION:
      api
        .get('/configuration')
        .send()
        .then((response) => {
          next({
            type: SET_CURRENCY_CONFIGURATION,
            data: {
              currencyPairs: response.data.currencyPairs,
            }
          });
        })
        .catch((error) => {
          alert(error);
        });

  }
};