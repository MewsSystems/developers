import { SET_CURRENCY_CONFIGURATION } from '../constants';

export default function (state = {}, action) {
  switch (action.type) {
    case SET_CURRENCY_CONFIGURATION:
      return {
        ...state,
        currency: state.currency,
      };
  }
}