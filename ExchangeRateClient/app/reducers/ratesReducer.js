import { GET_RATES_SUCCESS } from "./../actions/types";

const initialState = {
  rates: {},
  trends: {}
};

export default (state = initialState, action) => {
  switch (action.type) {
    case GET_RATES_SUCCESS: {
      const { rates } = action;
      console.log(rates);
      return { ...state, ...rates };
    }
    default:
      return state;
  }
};
