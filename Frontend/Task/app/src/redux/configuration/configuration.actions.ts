import {
  FETCH_CONFIGURATION_FAILURE,
  FETCH_CONFIGURATION_REQUEST,
  FETCH_CONFIGURATION_SUCCESS,
} from "./configuration.constants";
import {ConfigDispatch} from './configuration.models'

export const fetchConfigRequest = () => ({
  type: FETCH_CONFIGURATION_REQUEST
});
export const fetchConfigSuccess = config => ({
  type: FETCH_CONFIGURATION_SUCCESS,
  payload: config
});
export const fetchConfigFailure = error => ({
  type: FETCH_CONFIGURATION_FAILURE,
  payload: error
});

export const fetchConfigAsync = () => {
  return async (dispatch: ConfigDispatch) => {
        dispatch(fetchConfigRequest())
      try {
          const response = await fetch('http://localhost:3000/configuration')
          const data = await response.json()
          const {currencyPairs} = await data;
          let newConfig = {}
          Object.keys(currencyPairs).map(id => {
            const name = `${currencyPairs[id][0].name} / ${currencyPairs[id][1].name}`
            const code = `${currencyPairs[id][0].code} / ${currencyPairs[id][1].code}`
            return newConfig[id] = {
              name,
              code
            }
          })
        dispatch(fetchConfigSuccess(newConfig))
      } catch(err) {
        dispatch(fetchConfigFailure(err))
      }
  }
}
