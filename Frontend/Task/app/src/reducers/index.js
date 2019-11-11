import {combineReducers} from 'redux';
import config from "./ConfigReducer";
import currencyPairsValues from "./CurrencyPairsValuesReducer";
import filter from "./FilterReducer";

const exchangeAppReducer = combineReducers({
    config,
    currencyPairsValues,
    filter
});

export default exchangeAppReducer;