import { GET_CURRENCY_PAIRS_SUCCESS } from "./../actions/types";

const initialState = {};

export default (state = initialState, action) => {
  switch (action.type) {
    case GET_CURRENCY_PAIRS_SUCCESS: {
      const { currencyPairs } = action;
      console.log(currencyPairs);
      return {
        ...state,
        ...currencyPairs
      };
    }
    default:
      return state;
  }
};
