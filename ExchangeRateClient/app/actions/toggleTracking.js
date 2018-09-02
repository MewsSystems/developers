import { assocPath, path } from 'ramda';
import { actionTypes } from '../constants';
import { getCurrencyPairsById } from '../selectors';

export default (currencyPairId) => (dispatch, getState) => {
    const currencyPairsById = getCurrencyPairsById(getState());
    const pathToTrack = [currencyPairId, 'track'];
    const toggle = !path(pathToTrack, currencyPairsById);
    dispatch({ type: actionTypes.TOGGLE_TRACKING, payload: assocPath(pathToTrack, toggle, currencyPairsById) });
};
