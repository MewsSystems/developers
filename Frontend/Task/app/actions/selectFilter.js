import { actionTypes } from '../constants'

const filterItems = filter => ({
  type: actionTypes.FILTER_SELECTED,
  filter,
})

export const selectFilter = dispatch => filter => {
  dispatch(filterItems(filter))
}
