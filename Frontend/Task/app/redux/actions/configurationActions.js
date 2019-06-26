export const configurationActions = {
  FETCH_CONFIGURATION: 'configuration/FETCH_CONFIGURATION',
  FETCH_CONFIGURATION_SUCCESS: 'configuration/FETCH_CONFIGURATION_SUCCESS',
  FETCH_CONFIGURATION_FAILED: 'configuration/FETCH_CONFIGURATION_FAILED'
};

export const configurationActionCreators = {
  fetchConfiguration: () => ({ type: configurationActions.FETCH_CONFIGURATION }),
  fetchConfigurationSuccess: (configuration) => ({ type: configurationActions.FETCH_CONFIGURATION_SUCCESS, payload: { configuration } }),
  fetchConfigurationFailed: () => ({ type: configurationActions.FETCH_CONFIGURATION_FAILED }),
};