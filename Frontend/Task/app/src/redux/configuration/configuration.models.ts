import {
  FETCH_CONFIGURATION_FAILURE,
  FETCH_CONFIGURATION_SUCCESS,
  FETCH_CONFIGURATION_REQUEST
}
from './configuration.constants'
import {ThunkDispatch} from 'redux-thunk'
import {Action} from 'redux'

export interface IFetchConfigRequest {
  type: typeof FETCH_CONFIGURATION_REQUEST
}

export interface IFetchConfigSuccess {
  type: typeof FETCH_CONFIGURATION_SUCCESS
  payload: ConfigurationData
}

export interface IFetchConfigFailure {
  type: typeof FETCH_CONFIGURATION_FAILURE,
  payload: string,
}

export interface ConfigurationData {
  currencyPairs?: {
    currency: ServerData,
  }
}

export interface ServerData {
  currency: Array<ICurrency>
}

export interface ICurrency {
  code: string,
  name: string
}

export type ConfigReducerState = {
  configuration: {
    currencies: ConfigurationData,
    isLoading: boolean,
    error: string
  }
}

export type ConfigDispatch = ThunkDispatch<ConfigurationData, void, Action>