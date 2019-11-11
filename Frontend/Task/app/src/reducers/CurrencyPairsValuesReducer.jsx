export const RECEIVE_CURRENCY_PAIRS_VALUES = 'RECEIVE_CURRENCY_PAIRS_VALUES';

const currencyPairsValuesReducer = (state = [], action) => {
    switch (action.type) {
        case RECEIVE_CURRENCY_PAIRS_VALUES:
            const newState = [...state];
            newState.push(action.row);

            return newState;
        default:
            return state;
    }
};

export default currencyPairsValuesReducer;