import {getConfigurationUrl, getPairUrl} from "../constants/ExchangeRateConstants";
import {RECEIVE_CURRENCY_PAIRS_CONFIG} from "../reducers/CurrencyPairsConfigReducer";

const receiveCurrencyPairsConfig = (config) => {
    return {
        type: RECEIVE_CURRENCY_PAIRS_CONFIG,
        config
    }
};

export const fetchCurrencyPairsConfig = () => (dispatch) => {
    fetch(getConfigurationUrl(), {
        method: 'GET'
    }).then(response => {
        if (response.status === 200) {
            response.json().then(data => {
                dispatch(receiveCurrencyPairsConfig(data.currencyPairs))
            });
        }
    }).catch(error => {
        console.log(error);
    });
};

export const fetchCurrencyPairValue = (code) => {
    fetch(getPairUrl(code), {
        method: 'GET'
    }).then(response => {
        if (response.status === 200) {
            response.json().then(data => {
                console.log(data);
            });
        } else {
            console.log('erssssr');
        }
    }).catch(error => {
        console.log(error);
    });
};