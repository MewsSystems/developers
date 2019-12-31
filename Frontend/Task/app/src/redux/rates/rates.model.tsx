import {
  FETCH_RATES_FAILURE,
  FETCH_RATES_SUCCESS,
  FETCH_RATES_REQUEST
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
  rates?: IRate
}

export interface IRate {
  rate: number
}

export type RateReducerState = {
  rates: {
    ratesList: RatesData,
    isLoading: boolean,
    error: string
  }
}

export type RatesDispatch = ThunkDispatch<RatesData, void, Action>