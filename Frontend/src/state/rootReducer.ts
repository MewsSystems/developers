import { combineReducers } from 'redux'
import { PersistState } from 'redux-persist'
import {
  ConfigurationState,
  configurationInitialState,
} from './actions/configuration'
import { configurationReducer } from './reducers/configuration'

export interface State {
  configuration: ConfigurationState
  _persist?: PersistState
}

export const initialState: State = {
  configuration: configurationInitialState,
}

export const rootReducer = combineReducers({
  configuration: configurationReducer,
})
