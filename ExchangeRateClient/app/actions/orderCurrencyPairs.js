import { compose, head, map, path, reverse, sortBy, toPairs } from 'ramda';
import { actionTypes } from '../constants';
import { getCurrencyPairsById } from '../selectors';
import { currencyPairToString } from '../utils';

const sortStrategy = (sortField) => {
    switch (sortField) {
        case 'pair': return compose(currencyPairToString, path([1, sortField]));
        default: return path([1, sortField]);
    }
};

export default (arrangement, sortField) => (dispatch, getState) => {
    const sortedFields = compose(map(head), sortBy(sortStrategy(sortField)), toPairs, getCurrencyPairsById)(getState());
    dispatch({
        type: actionTypes.ORDER_CURRENCY_PAIRS,
        payload: arrangement === 'DESC' ? reverse(sortedFields) : sortedFields,
    });
};
