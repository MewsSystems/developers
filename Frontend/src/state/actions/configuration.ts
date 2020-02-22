import { Configuration } from 'model/api/Configuration'
import { createAction } from 'redux-actions'
import { ThunkDispatch } from 'redux-thunk'
import { AnyAction } from 'redux'
import { State } from 'state/rootReducer'
import { ConfigurationApi } from 'api/Configuration'

export type ConfigurationState = Configuration | null
export const configurationInitialState: ConfigurationState = null

export const HYDRATE_CONFIGURATION = 'HYDRATE_CONFIGURATION'

export const hydrateConfigurationAction = createAction<Configuration>(
  HYDRATE_CONFIGURATION
)

export const hydrateConfiguration = () => async (
  dispatch: ThunkDispatch<State, void, AnyAction>
) => {
  const { data } = await ConfigurationApi.getConfiguration()

  dispatch(hydrateConfigurationAction(data))
}
