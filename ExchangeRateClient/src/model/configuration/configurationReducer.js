import {SET_CONFIG} from './configurationActions';

const initData = {
    loaded: false,
    data: null,
}

export default function (state = initData, action) {
    if (action.type === SET_CONFIG) {
        return {
            loaded: true,
            data: action.data,
        };
    }

    return state;
}
