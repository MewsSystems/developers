import {
  PENDING, FULFILLED, REJECTED,
} from '../../../globals';
import { fetchData, } from '../../../utils/fetchData';


export const DATA__GET_RATES = 'DATA__GET_RATES';


/**
 * GET Rates
 */
export const getRatesAction = (currencyPairIds) => async (dispatch) => {
  try {
    dispatch({
      type: `${DATA__GET_RATES}__${PENDING}`,
    });

    const response = await fetchData({
      method: 'get',
      url: `${process.env.REACT_APP_API_URL}/rates`,
      params: {
        currencyPairIds,
      },
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
