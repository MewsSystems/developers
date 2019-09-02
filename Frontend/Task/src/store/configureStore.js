import { createStore, combineReducers, } from 'redux';
import { currencyPairsReducer } from 'reducers/currencyPairs'

const store = createStore(
  combineReducers({
    currencyPairs: currencyPairsReducer
  }))


export default () => store