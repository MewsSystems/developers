import * as actionTypes from '../actions/actionTypes';
// import * as R from "ramda";
const initialState = {};

const setInitialRates = (state, action) => {
  const newState = action.payload;
  const keys = Object.keys(action.payload);
  keys.forEach(
    key => (newState[key] = { rate: newState[key], trend: 'stagnating' })
  );
  return newState;
};

const updateRates = (state, action) => {
  const newState = action.payload;
  const keys = Object.keys(action.payload);
  const setTrend = (prev, next) => {
    if (prev > next) {
      return 'declining';
    }
    if (prev < next) {
      return 'growing';
    }
    return 'stagnating';
  };
  keys.forEach(
    key =>
      (newState[key] = {
        rate: newState[key],
        trend: setTrend(state[key].rate, newState[key])
      })
  );
  return newState;
};
const fetchFails = (state, action) => {
  console.log(action.payload);
  return state;
};
const reducer = (state = initialState, action) => {
  switch (action.type) {
    case actionTypes.FETCH_RATES_SUCCESS:
      return setInitialRates(state, action);
    case actionTypes.UPDATE_RATES:
      return updateRates(state, action);
    case actionTypes.FETCH_RATES_FAIL:
      return fetchFails(state, action);
    default:
      return state;
  }
};

export default reducer;
