import {
  PENDING, FULFILLED, REJECTED,
} from '../../../globals';
import { fetchData, } from '../../../utils.js/fetchData';
import { saveRatesConfigurationLS, getRatesConfigurationLS, } from '../../../utils.js/localStore';


/*
 * GET Rates Configuration
 *  - backup: localStorage
 */
export const DATA__GET_RATES_CONFIGURATION = 'DATA__GET_RATES_CONFIGURATION';
export const getRatesConfigurationAction = () => async (dispatch, getState) => {
  try {
    const { data: { ratesConfigurationReducer, }, } = getState();

    // if first time - check localStore for backup
    let lsData = null;
    if (!ratesConfigurationReducer.data && !ratesConfigurationReducer.error) {
      lsData = getRatesConfigurationLS();
    }

    // if backup fill store and send update request
    if (lsData) {
      dispatch({
        type: `${DATA__GET_RATES_CONFIGURATION}__${FULFILLED}`,
        payload: lsData,
      });
    } else {
      dispatch({
        type: `${DATA__GET_RATES_CONFIGURATION}__${PENDING}`,
      });
    }

    const response = await fetchData({
      method: 'get',
      url: `${process.env.REACT_APP_API_URL}/configuration`,
    });

    dispatch({
      type: `${DATA__GET_RATES_CONFIGURATION}__${FULFILLED}`,
      payload: response.data,
    });

    saveRatesConfigurationLS(response.data);
  } catch (error) {
    dispatch({
      type: `${DATA__GET_RATES_CONFIGURATION}__${REJECTED}`,
      payload: error,
    });
  }
};
