import {SET_FILTER} from './filterActions';

export default function (state = '', action) {
    if (action.type === SET_FILTER) {
        return action.filter;
    }

    return state;
}
