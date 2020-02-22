import { Action, handleActions } from 'redux-actions'
import { Configuration } from 'model/api/Configuration'
import {
  HYDRATE_CONFIGURATION,
  ConfigurationState,
  configurationInitialState,
} from 'state/actions/configuration'

export const configurationReducer = handleActions<ConfigurationState, any>(
  {
    [HYDRATE_CONFIGURATION]: (
      _state: ConfigurationState,
      action: Action<Configuration>
    ): ConfigurationState => {
      return action.payload
    },
  },
  configurationInitialState
)
