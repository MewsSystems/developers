import axios from './axios';
import { FETCH_CONFIGURATION, FETCH_RATES, REQSUEST_FAILED } from './actions';

export const fetchConfiguration = () => {
    return (dispatch) => {
        return axios.get('/configuration')
        .then(response => {
            dispatch(updateRatesAsync(response.data.currencyPairs));
            dispatch({
                type: FETCH_CONFIGURATION,
                currencyPairs: response.data.currencyPairs
            })
        }).catch(e => {
            dispatch({
                type: REQSUEST_FAILED
            })
          })
    };
};
export const fetchRates = (currencyPair) => {
    let currencyPairIds = [];
    Object.keys(currencyPair).map(key => currencyPairIds.push(key));
    return (dispatch) => {
        return axios.get('/rates', {
            params: {
                currencyPairIds: currencyPairIds
            }
        }).then(response => {
            dispatch({
                type: FETCH_RATES,
                rates: response.data.rates
            })
            return true;
        }).catch(e => {
            dispatch({
                type: REQSUEST_FAILED
            })
        })
    };
};

const updateRatesAsync = (currencyPair) => {
    return (dispatch) => {
        const updateRates = async () => {
            await dispatch(fetchRates(currencyPair));
            setTimeout(updateRates, 5000);
        };
        updateRates();          
    }
};