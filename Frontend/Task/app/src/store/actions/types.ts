import * as actionTypes from './actionTypes'
import { ConfigInterface, RateInterface } from '../../types'

// Config

export interface fetchConfigInitInterface {
  type: typeof actionTypes.FETCH_CONFIG_INIT
}

export interface checkConfigInterface {
  type: typeof actionTypes.CHECK_CONFIG
}

export interface fetchConfigInterface {
  type: typeof actionTypes.FETCH_CONFIG
}

export interface fetchConfigSuccessInterface {
  type: typeof actionTypes.FETCH_CONFIG_SUCCESS
  payload: ConfigInterface
}

export interface fetchConfigFailInterface {
  type: typeof actionTypes.FETCH_CONFIG_FAIL
  payload: string
}

// Filter

export interface filterCurrenciesInterface {
  type: typeof actionTypes.FILTER
  payload: string
}

export interface resetFilterInterface {
  type: typeof actionTypes.RESET_FILTER
}

// Rates

export interface fetchRatesInitInterface {
  type: typeof actionTypes.FETCH_RATES_INIT
  payload: RateInterface
}

export interface updateRatesSuccessInterface {
  type: typeof actionTypes.UPDATE_RATES_SUCCESS
  payload: RateInterface
}

export interface updateRatesInterface {
  type: typeof actionTypes.UPDATE_RATES
}

export interface fetchRatesFailInterface {
  type: typeof actionTypes.FETCH_RATES_FAIL
  payload: string
}

export interface fetchRatesRetryInterface {
  type: typeof actionTypes.FETCH_RATES_RETRY
}
