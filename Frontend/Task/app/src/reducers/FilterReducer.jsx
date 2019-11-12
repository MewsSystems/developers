import {INITIAL_STATE} from "./index";

export const RECEIVE_FILTER = 'RECEIVE_FILTER';

const filterReducer = (state = INITIAL_STATE.filter, action) => {
    switch (action.type) {
        case RECEIVE_FILTER:
            return action.filter;
        default:
            return state;
    }
};

export default filterReducer;