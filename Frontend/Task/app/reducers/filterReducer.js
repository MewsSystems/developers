import { actionTypes } from '../constants'
import { handleFilterSelection } from '../utils'

const initialState = {
  selectedFilter: null,
}

export default (state = initialState, action) => {
  switch (action.type) {
    case actionTypes.FILTER_SELECTED:
      const { filter } = action
      const { selectedFilter } = state

      return {
        ...state,
        selectedFilter: handleFilterSelection(filter, selectedFilter),
      }

    default:
      return state
  }
}
