import { combineReducers } from 'redux'
import pairReducer from './pairReducer'
import rateReducer from './rateReducer'
import filterReducer from './filterReducer'

const rootReducer = combineReducers({
  pairReducer,
  rateReducer,
  filterReducer,
})

export default rootReducer
