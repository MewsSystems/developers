export const GET_CURRENCY_PAIRS_LOADING = "GET_CURRENCY_PAIRS_LOADING";
export const GET_CURRENCY_PAIRS_SUCCESS = `GET_CURRENCY_PAIRS_SUCCESS`;
export const GET_CURRENCY_PAIRS_ERROR = `GET_CURRENCY_PAIRS_ERROR`;

export const GET_RATES_LOADING = "GET_RATES_LOADING";
export const GET_RATES_SUCCESS = `GET_RATES_SUCCESS`;
export const GET_RATES_ERROR = `GET_RATES_ERROR`;

const createAsyncAction = type => [
  `${type}_LOADING`,
  `${type}_SUCCESS`,
  `${type}_ERROR`
];
/*
export const [
  GET_RATES_LOADING,
  GET_RATES_SUCCESS,
  GET_RATES_ERROR,
] = createAsyncAction('GET_RATES');
*/
