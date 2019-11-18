import { SET_CURRENCY_CONFIGURATION } from '../constants';

const initState = {
  currencyPairs: null,
};

export default function (state = initState, action) {

  switch (action.type) {
    case SET_CURRENCY_CONFIGURATION:
      return {
        ...state,
        ...action.data,
      };

    default:
      return state;
  }
}