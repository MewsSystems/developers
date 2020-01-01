import {
  FETCH_RATES_FAILURE,
  FETCH_RATES_SUCCESS,
  FETCH_RATES_REQUEST,
  HTTP_500_ERROR}
from './rates.constants'
import {RatesData, IFetchRatesFailure, IFetchRatesRequest, IFetchRatesSuccess, IHTTP500Error} from './rates.model'

export type RatesState = {
    ratesList: RatesData,
    isLoading: boolean,
    errorMessage: string,
    showErrorAlert: boolean
}

export type RatesAction = IFetchRatesRequest | IFetchRatesSuccess | IFetchRatesFailure | IHTTP500Error


const INITIAL_STATE: RatesState = {
  ratesList: {},
  isLoading: true,
  errorMessage: '',
  showErrorAlert: false
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
        ratesList: action.payload,
        showErrorAlert: false
      }
    case FETCH_RATES_FAILURE:
      return {
        ...state,
        errorMessage: action.payload,
        showErrorAlert: false
      }
    case HTTP_500_ERROR:
      return {
        ...state,
        showErrorAlert: true
      }
    default:
      return state
  }

}