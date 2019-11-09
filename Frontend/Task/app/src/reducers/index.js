import {combineReducers} from 'redux';
import currencyPairsConfig from "./CurrencyPairsConfigReducer";

const exchangeAppReducer = combineReducers({
    currencyPairsConfig
});

export default exchangeAppReducer;