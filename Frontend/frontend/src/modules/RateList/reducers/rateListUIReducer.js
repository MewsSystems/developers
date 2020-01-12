const initialState = {
  /**
   * Table filter
   */
  filter: {
    currencyL: '',
    currencyR: '',
    sort: {
      name: '',
      order: '',
    },
  },
  /**
   * Unfiltered formated rows for table
   */
  unfilteredRows: [],
  /**
   * Table rows
   */
  rows: [],
};


/**
 * Rates List UI Reducer
 * @param {Object} state
 * @param {Object} action
 */
const reducer = (state = initialState, action) => {
  const { type, payload, } = action;
  switch (type) {
    default:
      return state;
  }
};

export default reducer;
