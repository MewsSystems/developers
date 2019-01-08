const initialState = {
  currencyPairs: null,
  selectedPairs: [],
  tableData: {},
  hasEverFetchedConfig: false,
};

const refactorPairs = pairsObject => {
  const ids = Object.keys (pairsObject);
  return ids.map (id => {
    const pair = pairsObject[id];
    return {
      id,
      pair,
      pairCode: pair[0].code + '/' + pair[1].code,
    };
  });
};

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case 'SET_CURRENCY_PAIRS': {
      const currencyPairs = refactorPairs (action.payload.currencyPairs);
      return {
        ...state,
        currencyPairs,
        selectedPairs: !state.hasEverFetchedConfig
          ? currencyPairs
              .map ((e, index) => (index < 5 ? e : null))
              .filter (e => !!e)
          : state.selectedPairs, // selecting first 5
        hasEverFetchedConfig: true,
      };
    }
    case 'TOGGLE_SELECTED_PAIR': {
      const {selectedPair} = action.payload;

      const selectedPairs = [...state.selectedPairs];
      const selectedPairIndex = selectedPairs
        .map (e => e.pairCode)
        .indexOf (selectedPair.pairCode);
      if (selectedPairIndex === -1) selectedPairs.push (selectedPair);
      else selectedPairs.splice (selectedPairIndex, 1);

      const selectionKey = Math.random ();
      // we will fetch data immediately as soon as we discover that
      // selectionKey has changed

      return {
        ...state,
        selectedPairs,
        selectionKey,
      };
    }
    case 'ADD_TABLE_DATA': {
      const timestamp = new Date ().getTime ();
      let tableData = {
        ...state.tableData,
        [timestamp]: {
          ...action.payload.tableData.rates,
        },
      };

      let columns = Object.keys (tableData);
      columns.sort ();
      columns.reverse ();
      if (columns.length > 5) {
        columns = columns.splice (columns.length - (5 + 1), 5);
        const newTableData = {};
        columns.map (e => {
          newTableData[e] = tableData[e];
        });
        tableData = newTableData;
      }

      return {
        ...state,
        tableData,
      };
    }
    default: {
      return state;
    }
  }
};

export default reducer;
