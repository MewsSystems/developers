import * as actionTypes from "./actionTypes";

export const saveTrendPairs = data => {
  return {
    type: actionTypes.SAVE__PAIRS,
    payload: data
  };
};

export const saveConfigurationData = currencies => {
  return {
    type: actionTypes.SAVE_CURRENCIES,
    payload: currencies
  };
};

export const saveFilterConfiguration = configuration => {
  return {
    type: actionTypes.SAVE_CONFIGURATION,
    payload: configuration
  };
};
