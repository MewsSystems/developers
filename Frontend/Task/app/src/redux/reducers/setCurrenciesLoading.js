// @flow strict

import type { State } from '../types';

const setCurrenciesLoading = (state: State): State => {
  return { ...state, isLoadingConfig: true };
};

export default setCurrenciesLoading;
