import { Movie } from '../types';

export const SET_SELECTED_RESULT = 'SET_SELECTED_RESULT';

const initialState = {};

const detailReducer = (
  state = initialState,
  action: { type: string; payload: { result?: Movie } },
) => {
  switch (action.type) {
    case SET_SELECTED_RESULT:
      return { ...action.payload.result };
    default:
      return state;
  }
};

export default detailReducer;
