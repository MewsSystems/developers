export const FETCH_CONFIGURATION = 'RATES/FETCH_CONFIGURATION';
export const FETCH_RATES = 'FETCH_RATES';
export const SET_RATES = 'SET_RATES';
export const ADD_RATE = 'ADD_RATE';
export const REMOVE_RATE = 'REMOVE_RATE';

export const addRate = rate => ({
  type: ADD_RATE,
  payload: rate,
});

// export const removeRate = rate => ({
//   type: REMOVE_RATE,
//   payload: rate,
// });

// export const setRates = rates => ({
//   type: SET_RATES,
//   payload: rates,
// });

export const fetchConfiguration = () => ({
  type: FETCH_CONFIGURATION,
  payload: fetch('http://localhost:3001/configuration').then(response =>
    response.json(),
  ),
});

export const fetchRates = currencyPairs => ({
  type: FETCH_RATES,
  payload: fetch(
    `http://localhost:3001/rates?currencyPairIds=${JSON.stringify(
      currencyPairs,
    )}`,
  ).then(response => response.json()),
});

// components:

// Currency pairs selector - Allows user to filter displayed currency pairs.
// Currency pairs rate list - Displays shortcut name, current value and trend for each selected currency pair. Shortcut is defined as {name1}/{name2}. Trend is defined as:
// growing, when prevValue < nextValue
// declining, when prevValue > nextValue
// stagnating, when prevValue == nextValue

// actions:
// () => currencyPairs
// fetchConfiguration

// [currencyPairIds] => rates
// fetchRates

// [currencyPairIds]
// setRates

// addRate

// removeRate

// reducer

// {
// currencyPairs,
// rates: {
//   previous,
//   current
// },
// selectedRates

// }
