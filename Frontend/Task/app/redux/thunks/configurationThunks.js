import { configurationActionCreators } from "../actions/configurationActions";
import ConfigurationService from '../../services/ConfigurationService';

export default {
  fetchConfiguration: () => async (dispatch) => {
    dispatch(configurationActionCreators.fetchConfiguration());

    try {
      const configuration = await ConfigurationService.fetchConfiguration();
      dispatch(configurationActionCreators.fetchConfigurationSuccess(configuration));
    } catch (e) {
      dispatch(configurationActionCreators.fetchConfigurationFailed());
    }
  }
};