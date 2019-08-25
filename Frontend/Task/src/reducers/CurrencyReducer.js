import * as types from '../actions/types';

const initialState = {
  configuration: [],
};


const CurrencyReducer = (state = initialState, action) => {
  switch (action.type) {
    case types.GET_CONFIGURATION_SUCCESS:
      return {
        ...state,
        configuration: action.configuration,
      };
    default:
      return state;
  }
};

export default CurrencyReducer;
