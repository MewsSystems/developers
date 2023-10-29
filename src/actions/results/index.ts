import { createAction } from '@reduxjs/toolkit';
import { UPDATE_RESULTS } from '../../reducers/resultsReducer';

export const updateResults = createAction(UPDATE_RESULTS, (results) => {
  return {
    payload: { results },
  };
});
