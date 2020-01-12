import {
  PENDING, FULFILLED, REJECTED,
} from '../../../globals';
import { fetchData, } from '../../../utils/fetchData';


export const DATA__GET_RATES = 'DATA__GET_RATES';


/*
 * GET Rates
 */
export const getRatesAction = () => async (dispatch, getState) => {
  try {
    const { data: { ratesConfiguration, }, } = getState();

    dispatch({
      type: `${DATA__GET_RATES}__${PENDING}`,
    });

    const response = await fetchData({
      method: 'get',
      url: `${process.env.REACT_APP_API_URL}/rates`,
      params: ratesConfiguration.data
        ? { currencyPairIds: Object.keys(ratesConfiguration.data), }
        : undefined,
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
