import { RATE_LIST__CHANGE_FILTER_VALUE, } from '../actions/rateListActions';
import { DATA__GET_RATES, } from '../../Main/dataActions/ratesActions';
import { DATA__GET_RATES_CONFIGURATION, } from '../../Main/dataActions/ratesConfigurationActions';
import { FULFILLED, SORT_UNSET, } from '../../../globals';
import { fulfilledRates, parseConfiguration, applyFilter, } from './utils';


const initialState = {
  /**
   * Table filter
   */
  filter: {
    values: {
      name: '',
    },
    sort: {
      name: null,
      order: SORT_UNSET,
    },
  },
  /**
   * Fetched Time
   */
  timestampConfiguration: null,
  /**
   * Unfiltered formated table rows
   */
  unfilteredRows: [],
  /**
   * Table rows
   */
  rows: [],
  /**
   * Rates
   */
  rates: {},
};


/**
 * Rates List UI Reducer
 */
const reducer = (state = initialState, action) => {
  const { type, payload, } = action;
  switch (type) {
    /**
     * Change Filter
     */
    case RATE_LIST__CHANGE_FILTER_VALUE: {
      return {
        ...state,
        filter: payload,
        rows: applyFilter(state.unfilteredRows, state.rates, payload),
      };
    }


    /**
     * Configuration Fulfilled
     */
    case `${DATA__GET_RATES_CONFIGURATION}__${FULFILLED}`: {
      const parsed = parseConfiguration(payload.currencyPairs);

      return {
        ...state,
        unfilteredRows: parsed,
        rows: applyFilter(parsed, state.rates, state.filter),
        timestampConfiguration: Date.parse(new Date()),
      };
    }


    /**
     * Rates Fulfilled
     */
    case `${DATA__GET_RATES}__${FULFILLED}`: {
      const newRates = fulfilledRates(state.rates, payload.rates);

      return {
        ...state,
        rates: newRates,
        rows: applyFilter(state.unfilteredRows, newRates, state.filter),
      };
    }


    default:
      return state;
  }
};

export default reducer;
