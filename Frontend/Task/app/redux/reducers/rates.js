import axios from 'axios';
import { keys, result } from 'lodash';
import Cookies  from 'js-cookie';
import {
    FETCH_CURRENCY_LIST,
    FETCH_CURRENCY_LIST_SUCCESS,
    FETCH_CURRENCY_LIST_ERROR,
    UPDATE_CURRENCY_RATES,
    UPDATE_CURRENCY_RATES_SUCCESS,
    UPDATE_CURRENCY_RATES_ERROR,
    SET_FILTER,
    SET_LOADED_CURRENCY_LIST
} from '../actionTypes';
import { TREND, ALL_CURRENCIES } from '../../constants/rates';
import { RATES_REQUEST_INTERVAL, ENDPOINT} from '../../configuration';

const COOKIE = {
    FILTER_BY: 'rates_filter_by',
    LIST: 'ratest_list'
};

const INITIAL_ERROR_STATE = {
    configurationList: null,
    rates: null
};

const initialState = {
    list: {},
    error: { ...INITIAL_ERROR_STATE},
    loading: false,
    loaded: false,
    filterBy: ALL_CURRENCIES
};

export const getFilterBy = state => {
    return result(state, 'rates.filterBy', undefined);
};

export const getList = state => {
    const filterValue = getFilterBy(state);

    if (keys(state.rates.list).length === 0) {
        return {};
    }

    if (state.rates.filterBy === 'ALL_CURRENCIES') {
        return result(state, 'rates.list', undefined)
    }

    const filteredItem = state.rates.list[filterValue];
    return {
        filterValue: filteredItem
    };
};

export const getLoading = state => {
    return result(state, 'rates.loading');
};

export const getLoaded = state => {
    return result(state, 'rates.loaded')
};

export const getError = state => {
    return result(state, 'rates.error');
};

const setFilterBy = currencyId => {
    return {
        type: SET_FILTER,
        payload: currencyId
    };
};

export const setFilterValue = filterValue => dispatch => {
  Cookies.set(COOKIE.FILTER_BY, filterValue);
  dispatch(setFilterBy(filterValue));
};

const setCurrencyConfiguration = data => {
    return {
        type: FETCH_CURRENCY_LIST_SUCCESS,
        payload: data
    }
};

const setLoadedCurrencyConfiguration = data => {
    return {
        type: SET_LOADED_CURRENCY_LIST,
        payload: data
    }
};

export const fetchConfigurationSuccess =  data => {
    const { currencyPairs } = data || {};
    const currencyData = keys(currencyPairs).reduce((summary,key) => {
        const firstPart = result(currencyPairs[key], '[0].code', '*');
        const secondPart = result(currencyPairs[key], '[1].code', '*');

        const currencyItem = {
            id: key,
            label: `${firstPart}/${secondPart}`,
            rate: 0
        };

        summary[key] = currencyItem;
        return summary;
    }, {});

    return setCurrencyConfiguration(currencyData);
};

export const initializeConfigurationList = () => dispatch => {
    const filterBy = Cookies.get(COOKIE.FILTER_BY) || ALL_CURRENCIES;
    let currencyConfiguration = Cookies.get(COOKIE.LIST) || '{}';
    currencyConfiguration = JSON.parse(currencyConfiguration);

    if (keys(currencyConfiguration).length === 0) {
        dispatch(fetchConfigurationList());
    } else {
        dispatch(setLoadedCurrencyConfiguration(currencyConfiguration));
        dispatch(fetchRates());
    }

    dispatch(setFilterBy(filterBy));
};

const fetchConfigurationList = () => dispatch => {
    dispatch({type: FETCH_CURRENCY_LIST });

    axios.get(ENDPOINT.CONFIGURATION)
        .then(response => {
            dispatch(fetchConfigurationSuccess(response.data));
        })
        .then(() => {
            dispatch(fetchRates());
        })
        .catch(error => {
            dispatch({ type: FETCH_CURRENCY_LIST_ERROR, error })
        });
};

const evaluateTrend = (previousValue, nextValue) => {
    if (previousValue < nextValue) {
        return TREND.GROWING;
    } else if (previousValue > nextValue) {
        return TREND.DECLINING;
    } else {
        return TREND.STAGNATING;
    }
};

export const fetchRatesSuccess = (ratesData, list) => {
    const updatedList = keys(list).reduce((data, key) => {
        const { id, label, rate: originalRate } = list[key];
        const trend = evaluateTrend(originalRate, ratesData[key]);

        data[key] = {
            id,
            label,
            rate: ratesData[key],
            trend
        };
        return data;
    }, {});

    Cookies.set(COOKIE.LIST, updatedList);
    return {
        type: UPDATE_CURRENCY_RATES_SUCCESS,
        payload: updatedList
    };
};

const processRatesRequest = ({ getState, dispatch }) => {
    const list = Object.assign({}, result(getState(), 'rates.list', {}));
    const ids = keys(list);

    dispatch({type: UPDATE_CURRENCY_RATES});

    axios.get(ENDPOINT.RATES, {params:{currencyPairIds: ids } })
        .then(response => {
            dispatch(fetchRatesSuccess(result(response, 'data.rates', {}), list));
        })
        .catch(error => {
            dispatch({type: UPDATE_CURRENCY_RATES_ERROR, error});
        });
};

export const fetchRates = () => (dispatch, getState) => {
    setInterval(() => {
        processRatesRequest({ dispatch, getState });
    }, RATES_REQUEST_INTERVAL)

};

export default function(state = initialState, action) {
    switch (action.type) {
        case FETCH_CURRENCY_LIST: {
            return {
                ...state,
                loading: true,
                loaded: false,
                error: { ...INITIAL_ERROR_STATE }
            };
        }
        case FETCH_CURRENCY_LIST_SUCCESS: {
            return {
                ...state,
                list: action.payload,
            };
        }
        case FETCH_CURRENCY_LIST_ERROR: {
            return {
                ...state,
                loaded: true,
                loading: false,
                error: { ...state.error,  configurationList: action.error }
            }
        }
        case SET_LOADED_CURRENCY_LIST: {
            return {
                ...state,
                list: action.payload,
                loaded: true,
                loading: false
            };
        }
        case UPDATE_CURRENCY_RATES: {
            return {
                ...state,
                error: { ...state.error, rates: null }
            }
        }
        case UPDATE_CURRENCY_RATES_SUCCESS: {
            return {
                ...state,
                list: action.payload,
                loading: false,
                loaded: true
            }
        }
        case UPDATE_CURRENCY_RATES_ERROR: {
            return {
                ...state,
                error: { ...state.error, rates: action.error }
            }
        }
        case SET_FILTER: {
            return {
                ...state,
                filterBy: action.payload
            }
        }
        default:
            return state;
    }
}
