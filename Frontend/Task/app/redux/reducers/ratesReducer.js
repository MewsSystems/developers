import { ratesActions } from "../actions/ratesActions";


const transformRates = (currentState, ratesDto) => {
  const newRates = {
    ...currentState.rates
  };

  Object.entries(ratesDto).forEach((pair) => {
    const [rateId, currentRate] = pair;
    const previousRate = (newRates[rateId] && newRates[rateId].currentRate) ? newRates[rateId].currentRate : null;

    newRates[rateId] = {
      previousRate,
      currentRate
    };
  });

  return newRates;
};

const defaultState = {
  loading: false,
  filter: '',
  rates: {}
};

const reducer = (state = defaultState, action) => {
  switch (action.type) {
    case ratesActions.UPDATE_RATES:
      return {
        ...state,
        loading: true
      }

    case ratesActions.UPDATE_RATES_SUCCESS:
      return {
        ...state,
        loading: false,
        rates: transformRates(state, action.payload.rates)
      }

    case ratesActions.UPDATE_RATES_FAILED:
      return {
        ...state,
        loading: false
      }

    case ratesActions.FILTER_RATES:
      return {
        ...state,
        filter: action.payload
      }

    default:
      return state;
  }
}

export default reducer;