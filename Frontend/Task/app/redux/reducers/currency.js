import { SET_CURRENCY } from '../constants';

export default function (state = {}, action) {
  switch (action.type) {
    case SET_CURRENCY:
      return {
        ...state,
        currency: state.currency,
      };
  }
}