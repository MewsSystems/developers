import { Action, handleActions } from 'redux-actions'
import {
  SET_SEARCH_VALUE,
  SearchState,
  searchInitialState,
} from 'state/actions/search'

export const searchReducer = handleActions<SearchState, any>(
  {
    [SET_SEARCH_VALUE]: (
      state: SearchState,
      action: Action<string>
    ): SearchState => ({
      ...state,
      value: action.payload,
    }),
  },
  searchInitialState
)
