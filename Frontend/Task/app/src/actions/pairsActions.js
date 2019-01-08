const setCurrencyPairs = currencyPairs => {
  return {
    type: 'SET_CURRENCY_PAIRS',
    payload: {
      currencyPairs,
    },
  };
};

const toggleSelectedPair = selectedPair => {
  return {
    type: 'TOGGLE_SELECTED_PAIR',
    payload: {
      selectedPair,
    },
  };
};

const addTableData = tableData => {
  return {
    type: 'ADD_TABLE_DATA',
    payload: {
      tableData,
    },
  };
};

export {setCurrencyPairs, toggleSelectedPair, addTableData};
