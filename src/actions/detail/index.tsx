import { createAction } from '@reduxjs/toolkit';
import { SET_SELECTED_RESULT } from '../../reducers/detailReducer';

export const setSelectedResult = createAction(SET_SELECTED_RESULT, (result) => {
  return { payload: { result } };
});
