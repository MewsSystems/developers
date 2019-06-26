import { configurationActions } from "../actions/configurationActions";

const defaultState = {
  loading: false,
  currencyPairs: {}
};

const reducer = (state = defaultState, action) => {
  switch (action.type) {
    case configurationActions.FETCH_CONFIGURATION:
      return {
        ...state,
        loading: true
      }

    case configurationActions.FETCH_CONFIGURATION_SUCCESS:
      return {
        ...state,
        loading: false,
        ...action.payload.configuration
      }

    case configurationActions.FETCH_CONFIGURATION_FAILED:
      return {
        ...state,
        loading: false
      }

    default:
      return state;
  }
}

export default reducer;