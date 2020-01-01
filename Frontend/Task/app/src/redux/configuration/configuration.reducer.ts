import * as actions from './configuration.constants'
import {ConfigurationData} from './configuration.models'
import {IFetchConfigRequest, IFetchConfigSuccess, IFetchConfigFailure} from './configuration.models'

export type ConfigState = {
    currencies: ConfigurationData,
    isLoading: boolean,
    errorMessage: string
}

export type ConfigAction = IFetchConfigRequest | IFetchConfigSuccess | IFetchConfigFailure

const INITIAL_STATE: ConfigState = {
  currencies: {},
  isLoading: false,
  errorMessage: ''
}

export default (state = INITIAL_STATE, action: ConfigAction) => {
  switch(action.type) {
    case actions.FETCH_CONFIGURATION_REQUEST:
      return {
          ...state,
          isLoading: true
      }
    case actions.FETCH_CONFIGURATION_SUCCESS:
      return {
        ...state,
        isLoading: false,
        currencies: action.payload
      }
    case actions.FETCH_CONFIGURATION_FAILURE:
      return {
        ...state,
        errorMessage: action.payload
      }
    default:
      return state
  }
}