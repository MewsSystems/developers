import * as types from "./types";
import { createDefaultFilter } from "./filter";
import { endpoint as ENDPOINT, interval as INTERVAL } from "./config.json";

export const setLoadingStatus = status => ({
  type: types.SET_LOADING_STATUS,
  status
});

export const getConfigurationAction = config => ({
  type: types.GET_CONFIG,
  config
});

export const getRatesAction = rates => ({
  type: types.GET_RATES,
  rates
});

export const getRatesData = data => ({
  type: types.GET_RATES_DATA,
  data
});

export const getCombinedRatesAction = combinedRates => ({
  type: types.GET_COMBINED_RATES,
  combinedRates
});

const connectionSimulator = dispatch => {
  return setTimeout(() => {
    dispatch(getRatesStoreUpdate());
  }, INTERVAL);
};

export const getConfiguration = () => {
  return async dispatch => {
    await dispatch(setLoadingStatus(true));

    const response = await fetch("http://localhost:3000/configuration");

    if (response.status === 200) {
      const config = await response.json();
      await dispatch(getConfigurationAction(config.currencyPairs));
      await dispatch(createDefaultFilter());
      await dispatch(setLoadingStatus(false));

      if (config.currencyPairs) {
        await dispatch(getRatesStoreUpdate(config.currencyPairs));
      }
    }
  };
};

export const getRatesStoreUpdate = () => {
  return async (dispatch, getState) => {
    const config = await getState().config;
    const prevState = await getState().rates;

    const configKeys = Object.keys(config);

    const queryString = configKeys
      .map((id, i) => {
        return `currencyPairIds[${i}]=${id}`;
      })
      .join("&");

    const res = await fetch(`${ENDPOINT}?${queryString}`);

    if (res.status === 200) {
      const json = await res.json();
      const rates = json.rates;
      await dispatch(getRatesAction(rates));

      const getTrend = id => {
        if (prevState[id]) {
          const prevValue = prevState[id];
          const nextValue = rates[id];
          if (prevValue < nextValue) return "growing";
          if (prevValue > nextValue) return "declining";
          if (prevValue === nextValue) return "stagnating";
        } else {
          return null;
        }
      };

      const combinedRates = configKeys.map(id => {
        return {
          id: id,
          config: config[id],
          rate: rates[id],
          trend: getTrend(id)
        };
      });

      await dispatch(getCombinedRatesAction(combinedRates));
      await dispatch(getRatesDataAction());
      connectionSimulator(dispatch);
    } else {
      dispatch(getRatesStoreUpdate());
    }
  };
};

//COMPONENTS DATA
export const getRatesDataAction = () => {
  return async (dispatch, getState) => {
    const { filter, combinedRates } = await getState();
    await dispatch(
      getRatesData(
        combinedRates.filter(rate => {
          return filter[rate.id];
        })
      )
    );
  };
};
