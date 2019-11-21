import {tableColumns} from '../config';

const initialState = {
  currencyPairs: null,
  selectedPairs: [],
  tableData: {},
  hasEverFetchedConfig: false,
};

const reducer = (state = initialState, action) => {
  const {payload} = action;
  switch (action.type) {
    case 'SET_CURRENCY_PAIRS': {
      return setPairs ({state, payload});
    }
    case 'TOGGLE_SELECTED_PAIR': {
      return togglePair ({state, payload});
    }
    case 'ADD_TABLE_DATA': {
      return addData ({state, payload});
    }
    default:
      return state;
  }
};

export default reducer;

function addData({state, payload}) {
  const timestamp = new Date ().getTime ();
  const newRecord = {
    [timestamp]: {
      ...payload.tableData.rates,
    },
  };
  let tableData = {
    ...state.tableData,
    ...newRecord,
  };

  const columns = Object.keys (tableData).sort ().reverse ();
  const indexLatest = Math.max (0, columns.length - (tableColumns + 1));
  const latestColumns = columns.slice (indexLatest, tableColumns);

  return {
    ...state,
    tableData: latestColumns.reduce ((data, record) => {
      data[record] = tableData[record];
      return data;
    }, {}),
  };
}

function togglePair({state, payload}) {
  const {selectedPair} = payload;
  const {code} = selectedPair;

  const selectedPairs = [...state.selectedPairs];
  const selected = selectedPairs.map (e => e.code);
  const toggled = selected.includes (code);

  if (toggled) selectedPairs.splice (selected.indexOf (code), 1);
  else selectedPairs.push (selectedPair);

  return {
    ...state,
    selectedPairs,
    selectionKey: Math.random (),
  };
}

function setPairs({state, payload}) {
  const pairs = formatPairs (payload.currencyPairs);
  const hasSeenConfig = state.hasEverFetchedConfig;

  const getFirstPairs = () => pairs.slice (0, Math.max (1, Math.floor(pairs.length / 2)));
  const pairsPrev = state.selectedPairs;
  const selectedPairs = hasSeenConfig ? pairsPrev : getFirstPairs ();

  return {
    ...state,
    currencyPairs: pairs,
    selectedPairs,
    hasEverFetchedConfig: true,
  };
}

function formatPairs (pairsObject) {
  return Object.keys (pairsObject).map (id => {
    const pair = pairsObject[id];
    const code = pair[0].code + '/' + pair[1].code;
    return {
      id,
      pair,
      code,
    };
  });
}
