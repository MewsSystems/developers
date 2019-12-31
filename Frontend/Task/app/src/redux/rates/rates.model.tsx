import {
  FETCH_RATES_FAILURE,
  FETCH_RATES_SUCCESS,
  FETCH_RATES_REQUEST,
  FETCH_RATES_RETRY,
  SEARCH_RATES
}
from './rates.constants'
import {ThunkDispatch} from 'redux-thunk'
import {Action} from 'redux'

export interface IFetchRatesRequest {
  type: typeof FETCH_RATES_REQUEST,
}

export interface IFetchRatesSuccess {
  type: typeof FETCH_RATES_SUCCESS,
  payload: RatesData
}

export interface IFetchRatesFailure {
  type: typeof FETCH_RATES_FAILURE,
  payload: string
}

export interface RatesData {
  ratesList?: IRate
}

export interface IRate {
  name: string,
  code: string,
  currentRate: number,
  previousRate: number,
  trend: string,
}

export type RateReducerState = {
  rates: {
    ratesList: RatesData,
    isLoading: boolean,
    error: string
  }
  fetchRates: Function,
}

export type RatesDispatch = ThunkDispatch<RateReducerState, void, Action>