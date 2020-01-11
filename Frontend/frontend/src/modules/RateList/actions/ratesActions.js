import {
  PENDING, FULFILLED, REJECTED,
} from '../../../globals';
import { fetchData, } from '../../../utils.js/fetchData';


/*
 * GET Rates
 */
export const DATA__GET_RATES = 'DATA__GET_RATES';
export const getRatesAction = () => async (dispatch, getState) => {
  try {
    const { data: { ratesConfigurationReducer, }, } = getState();

    dispatch({
      type: `${DATA__GET_RATES}__${PENDING}`,
    });

    const response = await fetchData({
      method: 'get',
      url: `${process.env.REACT_APP_API_URL}/rates`,
      params: { currencyPairIds: Object.keys(ratesConfigurationReducer.data), },
    });

    dispatch({
      type: `${DATA__GET_RATES}__${FULFILLED}`,
      payload: response.data,
    });
  } catch (error) {
    dispatch({
      type: `${DATA__GET_RATES}__${REJECTED}`,
      payload: error,
    });
  }
};
