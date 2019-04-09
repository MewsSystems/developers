import { createStore, combineReducers } from 'redux'

import config from './config'
import rates from './rates'

const reducer = combineReducers({
  config,
  rates,
})

const store = createStore(reducer)

export default store
