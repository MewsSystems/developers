import {
  PENDING, FULFILLED, REJECTED,
} from '../../../globals';
import { fetchData, } from '../../../utils/fetchData';


export const DATA__GET_RATES_CONFIGURATION = 'DATA__GET_RATES_CONFIGURATION';


/*
 * GET Rates Configuration
 */
export const getRatesConfigurationAction = () => async (dispatch) => {
  try {
    dispatch({
      type: `${DATA__GET_RATES_CONFIGURATION}__${PENDING}`,
    });

    const response = await fetchData({
      method: 'get',
      url: `${process.env.REACT_APP_API_URL}/configuration`,
    });

    dispatch({
      type: `${DATA__GET_RATES_CONFIGURATION}__${FULFILLED}`,
      payload: response.data,
    });
  } catch (error) {
    dispatch({
      type: `${DATA__GET_RATES_CONFIGURATION}__${REJECTED}`,
      payload: error,
    });
  }
};
