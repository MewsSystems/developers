import { Reducer } from 'redux'
import { MovieIndexState, MovieIndexActionTypes } from './types'

// Type-safe initialState!
export const initialState: MovieIndexState = {
  data: undefined,
  errors: undefined,
  loading: false
}

// Thanks to Redux 4's much simpler typings, we can take away a lot of typings on the reducer side,
// everything will remain type-safe.
const reducer: Reducer<MovieIndexState> = (state = initialState, action) => {
  switch (action.type) {
    case MovieIndexActionTypes.SEARCH_CHANGED: {
      return { ...state, search: action.payload, page: 1, loading: !!action.payload }
    }
    case MovieIndexActionTypes.PAGE_CHANGED: {
      return { ...state, page: action.payload, loading: true }
    }
    case MovieIndexActionTypes.FETCH_REQUEST: {
      return { ...state, loading: true }
    }
    case MovieIndexActionTypes.FETCH_SUCCESS: {
      return { ...state, loading: false, data: action.payload }
    }
    case MovieIndexActionTypes.FETCH_ERROR: {
      return { ...state, loading: false, errors: action.payload }
    }
    default: {
      return state
    }
  }
}

// Instead of using default export, we use named exports. That way we can group these exports
// inside the `index.js` folder.
export { reducer as movieIndexReducer }
