export const RECEIVE_FILTER = 'RECEIVE_FILTER';

const filterReducer = (state = '', action) => {
    switch (action.type) {
        case RECEIVE_FILTER:
            return action.filter;
        default:
            return state;
    }
};

export default filterReducer;