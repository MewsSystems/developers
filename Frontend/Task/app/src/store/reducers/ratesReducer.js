import * as actionTypes from '../actions/actionTypes';
// import * as R from "ramda";
const initialState = {};

const updateRates = (state, action) => {
  const newState = action.payload;
  console.log('new state ', newState);
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
  keys.forEach(key => {
    if (state[key]) {
      return (newState[key] = {
        rate: newState[key],
        trend: setTrend(state[key].rate, newState[key])
      });
    }
    return (newState[key] = {
      rate: newState[key],
      trend: 'stagnating'
    });
  });
  return newState;
};
const fetchFails = (state, action) => {
  return state;
};
const reducer = (state = initialState, action) => {
  console.log('[ratesReducer]', action);
  switch (action.type) {
    case actionTypes.UPDATE_RATES_SUCCESS:
      return updateRates(state, action);
    case actionTypes.FETCH_RATES_FAIL:
      return fetchFails(state, action);
    default:
      return state;
  }
};

export default reducer;
