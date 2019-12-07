import {
  SAVE_CONFIG,
  REQUEST_CONFIG,
  CONFIG_ERROR,
  TRENDS,
  REQUEST_UPDATE,
  UPDATE_ERROR,
  SAVE_UPDATE,
  SET_FILTERS
} from "./constants";

export const fetchConfig = () => async dispatch => {
  dispatch(requestConfig());
  try {
    const response = await fetch("http://localhost:3000/configuration");
    const json = await response.json();
    const currencyPairs = {};
    const filteredPairs = [];
    Object.entries(json.currencyPairs).forEach(([id, value]) => {
      currencyPairs[id] = {
        shortcut: `${value[0].code}/${value[1].code}`,
        value: 0,
        trend: "N/A"
      };
      filteredPairs.push(id);
    });
    dispatch(saveConfig(currencyPairs, filteredPairs));
  } catch (error) {
    dispatch(setError("Fetching failed. Please reload the page."));
  }
};

export const requestConfig = () => ({ type: REQUEST_CONFIG });
export const saveConfig = (currencyPairs, filteredPairs) => ({
  type: SAVE_CONFIG,
  currencyPairs,
  filteredPairs
});
export const setError = error => ({ type: CONFIG_ERROR, error });

export const fetchRates = () => async (dispatch, getState) => {
  dispatch(requestUpdate());
  try {
    const { currencyPairs } = getState();
    const urlParams = new URLSearchParams();
    Object.keys(currencyPairs).forEach(id => {
      urlParams.append("currencyPairIds", id);
    });
    const response = await fetch(
      `http://localhost:3000/rates?${urlParams.toString()}`
    );
    const json = await response.json();
    const newData = {};
    Object.entries(currencyPairs).forEach(([id, pair]) => {
      const newValue = json.rates[id];
      let trend;
      if (pair.value === 0) {
        trend = "N/A";
      } else if (pair.value > newValue) {
        trend = TRENDS.DECLINING;
      } else if (pair.value === newValue) {
        trend = TRENDS.STAGNATING;
      } else if (pair.value < newValue) {
        trend = TRENDS.GROWING;
      }

      newData[id] = {
        ...pair,
        value: newValue,
        trend
      };
    });
    dispatch(saveUpdate(newData));
  } catch (error) {
    dispatch(updateError("Updating data failed."));
  }
};

export const requestUpdate = () => ({ type: REQUEST_UPDATE });
export const saveUpdate = currencyPairs => ({
  type: SAVE_UPDATE,
  currencyPairs
});
export const updateError = error => ({ type: UPDATE_ERROR, error });

export const firstLoad = () => async (dispatch, getState) => {
  const { currencyPairs } = getState();
  if (Object.keys(currencyPairs).length === 0) {
    await dispatch(fetchConfig());
  }
  await dispatch(fetchRates());
};

export const setFilters = filteredPairs => ({
  type: SET_FILTERS,
  filteredPairs
});

export const updateFilters = id => (dispatch, getState) => {
  const { filteredPairs } = getState();
  const newFilteredPairs = !filteredPairs.includes(id)
    ? [...filteredPairs, id]
    : filteredPairs.filter(pairId => pairId !== id);

  dispatch(setFilters(newFilteredPairs));
};
