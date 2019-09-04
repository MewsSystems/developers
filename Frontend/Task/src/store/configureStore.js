import { createStore, combineReducers, } from 'redux';
import { currencyPairsReducer } from 'reducers/currencyPairs'
import { filtersReducer } from 'reducers/filters'

const store = createStore(
  combineReducers({
    currencyPairs: currencyPairsReducer,
    filters: filtersReducer,
  }))


export default () => store