import initialState from './initialState'
import {
  FETCH_CONFIG_REQUEST,
  FETCH_CONFIG_SUCCESS,
  FETCH_CONFIG_ERROR,
  FETCH_RATES_REQUEST,
  FETCH_RATES_SUCCESS,
  FETCH_RATES_ERROR,
  PAIR_TOGGLE,
} from '../constants/actionTypes'

const ACTION_HANDLERS = {
  [FETCH_CONFIG_REQUEST]  : (state, action) => ({ ...state, isConfigFetching: true }),
  [FETCH_CONFIG_SUCCESS]  : (state, action) => (
    { ...state, isConfigFetching: false, config: action.payload }
  ),
  [FETCH_CONFIG_ERROR]    : (state, action) => ({ ...state, isConfigFetching: false }),
  //-----------------------------------------------------------
  [FETCH_RATES_SUCCESS]  : (state, action) => {
    var tmp = JSON.parse(JSON.stringify(state)).config;
    Object.keys(action.payload.rates).forEach( o => {
      tmp[o].oldRate = tmp[o].newRate
      tmp[o].newRate = action.payload.rates[o]
    })
    return({...state, config: tmp})
  },
  //-----------------------------------------------------------
  [PAIR_TOGGLE]           : (state, action) => {
    var tmp = JSON.parse(JSON.stringify(state));
    tmp.config[action.payload].selected = !tmp.config[action.payload].selected;
    if(!tmp.config[action.payload].selected){
      tmp.config[action.payload].oldRate = null
      tmp.config[action.payload].newRate = null
    }
    return(tmp)
  }
}


export default function configReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}