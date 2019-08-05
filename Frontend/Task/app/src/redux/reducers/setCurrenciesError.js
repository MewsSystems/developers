// @flow strict

import type { State, Action } from '../types';

const setCurrenciesError = (state: State, { payload }: Action): State => {
  const { error } = payload;
  if (!error) {
    return state;
  }

  return {
    ...state,
    isLoadingConfig: false,
    fetchConfigError: error,
  };
};

export default setCurrenciesError;
