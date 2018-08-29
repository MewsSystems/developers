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
import { _reduce, _mapValues } from '../utils/lodash'

const ACTION_HANDLERS = {
  [FETCH_CONFIG_REQUEST]  : (state, action) => ({ ...state, isConfigFetching: true }),
  [FETCH_CONFIG_SUCCESS]  : (state, action) => (
    { ...state, isConfigFetching: false, config: action.payload }
  ),
  [FETCH_CONFIG_ERROR]    : (state, action) => ({ ...state, isConfigFetching: false }),
  //-----------------------------------------------------------
  [FETCH_RATES_SUCCESS]  : (state, action) => {
    const newConfig = _reduce((res, val, id) => {
      res[id] = {...val, oldRate: val.newRate, newRate: action.payload.rates[id]}
      return res
    }, {}, state.config)
    return({...state, config: newConfig})
  },
  //-----------------------------------------------------------
  [PAIR_TOGGLE]           : (state, action) => {
    const newConfig = _mapValues((val, id) => {
      if(id === action.payload){
        if(val.selected) return {...val, selected: false}
        else return {...val, selected: true, oldRate: null, newRate: null}
      } else return {...val}
    }, state.config)
    return({...state, config: newConfig})
  }
}

export default function configReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}