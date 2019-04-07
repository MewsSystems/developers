import { RATES } from '../constants/actionTypes';
import { getRates } from '../actions/rates';

/**
 * When user change selected currencies pair we don't want to wait until next get rates tick, we
 * want to get new rates immediately
 * @param dispatch
 * @param getState
 * @returns {function(*): function(*=)}
 */
const immediateRatesFetcher = ({ dispatch }) => next => action => {
  if (action.type === `SET_${RATES}`) {
    const { payload } = action;

    dispatch(getRates(payload.map(item => item.value)));
  }
  return next(action);
};

export default immediateRatesFetcher;
