import {
  PENDING, FULFILLED, REJECTED,
} from '../../../globals';
import { fetchData, } from '../../../utils/fetchData';
import { setItemLS, getItemLS, LS__RATES_CONFIGURATION, } from '../../../utils/localStore';


export const DATA__GET_RATES_CONFIGURATION = 'DATA__GET_RATES_CONFIGURATION';


/*
 * GET Rates Configuration
 *  - backup: localStorage
 */
export const getRatesConfigurationAction = () => async (dispatch, getState) => {
  try {
    const { data: { ratesConfiguration, }, } = getState();

    // if first time - check localStore for backup
    let lsData = null;
    if (!ratesConfiguration.data && !ratesConfiguration.error) {
      lsData = getItemLS(LS__RATES_CONFIGURATION);
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

    setItemLS(LS__RATES_CONFIGURATION, response.data);
  } catch (error) {
    dispatch({
      type: `${DATA__GET_RATES_CONFIGURATION}__${REJECTED}`,
      payload: error,
    });
  }
};
