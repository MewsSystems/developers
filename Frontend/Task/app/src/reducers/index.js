import {combineReducers} from 'redux';
import config from "./ConfigReducer";
import currencyPairsValues from "./CurrencyPairsValuesReducer";
import filter from "./FilterReducer";

export const INITIAL_STATE = {
    config: null,
    currencyPairsValues: [],
    filter: ''
};

const exchangeAppReducer = combineReducers({
    config,
    currencyPairsValues,
    filter
});

export default exchangeAppReducer;