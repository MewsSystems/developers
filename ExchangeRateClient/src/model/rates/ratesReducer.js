import {SET_RATES} from './ratesActions';

const initData = {
    previousData: null,
    data: null,
}

export default function (state = initData, action) {
    if (action.type === SET_RATES) {
        return {
            previousData: state.data,
            data: action.data,
        };
    }

    return state;
}
