import constants from '../constants';

export const fetchConfig = () => ({
  type: constants.FETCH_CONFIG,
  payload: {
    promise: fetch(`/configuration`, {
      method: 'GET'
    })
    .then(response => {
      if (response.status !== 200) {
        throw new Error(response);
      }
        return response.json();
    })
    .catch(reason => throw new Error (reason))
  }
});

export const fetchRate = ids => ({
  type: constants.FETCH_RATES,
    payload: {
      promise: fetch(`/rates?currencyPairIds=${ids}`)
      .then(response => {
        if (response.status !== 200) {
          throw new Error(response);
        }
        return response.json();
      })
      .catch(response => {
        throw new Error(response)
      })
    }
});

export const clearData = () => ({
  type: constants.CLEAR_DATA
});

export const setSelectValue = value => ({
  type: constants.SET_SELECT_VALUE,
  payload: value
});

export const setIntervalId = id => ({
  type: constants.SET_INTERVAL_ID,
  payload: id
});

export const restoreConfig = (config, selectedPair) => ({
  type: constants.RESTORE_CONFIG,
  payload: { config, selectedPair }
});

export const setTimerIntervalId = (countDownId) => ({
  type: constants.SET_TIMER_INTERVAL_ID,
  payload: countDownId
});
