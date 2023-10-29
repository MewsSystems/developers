import { createAction } from '@reduxjs/toolkit';
import {
  TRIGGER_SEARCH,
  NEXT_PAGE,
  PREV_PAGE,
  SPECIFIC_PAGE,
  SET_QUERY_PARAMS,
} from '../../reducers/searchReducer';

export const triggerSearch = createAction(TRIGGER_SEARCH, (query) => {
  return {
    payload: { query },
  };
});

export const setQueryParams = createAction(SET_QUERY_PARAMS, (param) => {
  return { payload: { queryParams: param } };
});

export const fetchNextPage = createAction(NEXT_PAGE);
export const fetchPrevPage = createAction(PREV_PAGE);
export const fetchSpecificPage = createAction(SPECIFIC_PAGE, (page) => {
  return { payload: { page } };
});
