import {
  FETCH_RATES_FAILURE,
  FETCH_RATES_SUCCESS,
  FETCH_RATES_REQUEST}
from './rates.constants'
import {RatesData, IFetchRatesFailure, IFetchRatesRequest, IFetchRatesSuccess} from './rates.model'

export type RatesState = {
    ratesList: RatesData,
    isLoading: boolean,
    error: string
}

export type RatesAction = IFetchRatesRequest | IFetchRatesSuccess | IFetchRatesFailure


const INITIAL_STATE: RatesState = {
  ratesList: {},
  isLoading: false,
  error: ''
}

export default (state = INITIAL_STATE, action: RatesAction) => {
  switch(action.type) {
    case FETCH_RATES_REQUEST:
      return {
        ...state,
        isLoading: true
      }
    case FETCH_RATES_SUCCESS:
      return {
        ...state,
        isLoading: false,
        ratesList: action.payload
      }
    case FETCH_RATES_FAILURE:
      return {
        ...state,
        error: action.payload
      }
    default:
      return state
  }

}