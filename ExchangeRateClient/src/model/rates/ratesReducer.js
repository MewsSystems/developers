import {SET_RATES} from './ratesActions';
import isEqual from 'lodash/isEqual';

const initData = {
    previousData: null,
    data: null,
}

export default function (state = initData, action) {
    if (action.type === SET_RATES) {
        if (isEqual(state.data, action.data)) {
            // no change happened
            return state;
        }

        return {
            previousData: state.data,
            data: action.data,
        };
    }

    return state;
}
