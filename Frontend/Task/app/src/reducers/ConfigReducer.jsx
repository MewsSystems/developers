import {INITIAL_STATE} from "./index";

export const RECEIVE_CURRENCY_PAIRS_CONFIG = 'RECEIVE_CURRENCY_PAIRS_CONFIG';

const configReducer = (state = INITIAL_STATE.config, action) => {
    switch (action.type) {
        case RECEIVE_CURRENCY_PAIRS_CONFIG:
            return action.config;
        default:
            return state;
    }
};

export default configReducer;