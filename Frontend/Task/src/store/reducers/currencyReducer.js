import * as actionTypes from "../actions/actionTypes";

const initialState = {
  trendPairs: [],
  allCurrencies: [],
  selectConfiguration: []
};

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case actionTypes.SAVE__PAIRS:
      return {
        ...state,
        trendPairs: [...action.payload]
      };
    case actionTypes.SAVE_CURRENCIES:
      return {
        ...state,
        allCurrencies: [...action.payload]
      };
    case actionTypes.SAVE_CONFIGURATION:
      return {
        ...state,
        selectConfiguration: [...action.payload]
      };
    default:
      return state;
  }
};

export default reducer;
