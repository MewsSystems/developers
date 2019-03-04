export const FETCH_CONFIGURATION = 'RATES/FETCH_CONFIGURATION';
export const FETCH_RATES = 'FETCH_RATES';
export const SET_RATES = 'SET_RATES';

export const setRates = rates => ({
  type: SET_RATES,
  payload: rates,
});

export const fetchConfiguration = () => {
  return (dispatch, _, { getConfig }) => {
    dispatch({
      type: FETCH_CONFIGURATION,
      payload: fetch(`${getConfig().endpoint}/configuration`)
        .then(response => response.json())
        .catch(error => console.log(error)),
    });
  };
};

const arrayToParams = (property, list) => {
  return list.reduce((acc, cur, idx) => {
    if (idx === 0) {
      return `${property}[]=${cur}`;
    }
    return `${acc}&${property}[]=${cur}`;
  }, '');
};

function handleErrors(response) {
  if (!response.ok) {
    throw Error(response.statusText);
  }
  return response;
}

export const fetchRates = currencyPairs => {
  return (dispatch, _, { getConfig }) => {
    dispatch({
      type: FETCH_RATES,
      payload: fetch(
        `${getConfig().endpoint}/rates?${arrayToParams(
          'currencyPairIds',
          currencyPairs,
        )}`,
      )
        .then(handleErrors)
        .then(response => response.json())
        .catch(error => console.log(error)),
    });
  };
};
