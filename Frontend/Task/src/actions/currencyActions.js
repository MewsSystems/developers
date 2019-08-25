import * as types from './types';

// LIST
export function getConfigurationSuccess(configuration) {
  return {
    type: types.GET_CONFIGURATION_SUCCESS,
    configuration,
  };
}
