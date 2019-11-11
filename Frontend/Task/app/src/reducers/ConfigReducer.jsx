export const RECEIVE_CURRENCY_PAIRS_CONFIG = 'RECEIVE_CURRENCY_PAIRS_CONFIG';

const configReducer = (state = null, action) => {
    switch (action.type) {
        case RECEIVE_CURRENCY_PAIRS_CONFIG:
            return action.config;
        default:
            return state;
    }
};

export default configReducer;