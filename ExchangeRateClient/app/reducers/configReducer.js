import { actionTypes } from '../constants';

export default (initialState = {}, action) => {
    switch (action.type) {
        case actionTypes.SAVE_SETTINGS:
            return action.payload;
        default: return initialState;
    }
};
