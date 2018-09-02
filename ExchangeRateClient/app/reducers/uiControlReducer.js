import { actionTypes } from '../constants';

export default (initialState = {}, action) => {
    switch (action.type) {
        case actionTypes.SHOW_COUNTDOWN: {
            return { ...initialState, showCountdown: true };
        }
        case actionTypes.UPDATE_CURRENT_PAGE: {
            return action.payload;
        }
        default: return initialState;
    }
};
