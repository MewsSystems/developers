export const CONSTANTS = {
  TOGGLE_FILTER: 'TOGGLE_FILTER',
  UPDATE_FILTER_VALUE: 'UPDATE_FILTER_VALUE',
}

export function toggleFilter (isFilterEnabled = false) {
  return {
    type: CONSTANTS.TOGGLE_FILTER,
    payload: {
      isFilterEnabled,
    },
  }
}

export function updateFilterValue (value = '') {
  return {
    type: CONSTANTS.UPDATE_FILTER_VALUE,
    payload: {
      value,
    },
  }
}
