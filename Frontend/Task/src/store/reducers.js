import _ from 'lodash';
import { getCurrencies } from 'helpers';
import state from 'store/state';

const initialState = state;

export default (state = initialState, action) => {
  const { data, error } = action;

  switch (action.type) {
    case 'CONFIG_REQUEST':
      _.set(state, 'configuration', { ...state.configuration, loading: true, error: null });
      return { ...state };
    case 'CONFIG_SUCCESS':
      _.set(state, 'configuration', { prevData: state.configuration.data, data, loading: false, error: null });
      _.set(state, 'currencies', getCurrencies(data));
      return { ...state };
    case 'CONFIG_ERROR':
      _.set(state, 'configuration', { ...state.configuration, loading: false, error });
      return { ...state };
    case 'RATES_REQUEST':
      _.set(state, 'rates', { ...state.rates, loading: true, error: null });
      return { ...state };
    case 'RATES_SUCCESS':
      _.set(state, 'rates', { prevData: state.rates.data, data, loading: false, error: null });
      return { ...state };
    case 'RATES_ERROR':
      _.set(state, 'rates', { ...state.rates, loading: false, error });
      return { ...state };
    case 'UPDATE_FILTER':
      state.filter = [...data];
      return { ...state };
    default:
      return state;
  };
};