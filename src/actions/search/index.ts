import { createAction } from '@reduxjs/toolkit';
import { TRIGGER_SEARCH } from '../../reducers/searchReducer';

export const triggerSearch = createAction(TRIGGER_SEARCH, (query) => {
  return {
    payload: { query },
  };
});
