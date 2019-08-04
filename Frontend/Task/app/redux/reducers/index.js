// @flow strict

import { INITIAL_STATE, ACTIONS } from '../constants';
import type { Action, State } from '../types';
import sanitizeCurrencies from './sanitizeCurrencies';

const rootReducer = (state: State = INITIAL_STATE, action: Action): State => {
  switch (action.type) {
    case ACTIONS.SANITIZE_CURRENCIES:
      return sanitizeCurrencies(state, action);

    default:
      return state;
  }
};

export default rootReducer;
