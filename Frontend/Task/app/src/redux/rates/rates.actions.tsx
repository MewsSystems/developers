import {
  FETCH_RATES_FAILURE,
  HTTP_500_ERROR,
  FETCH_RATES_REQUEST,
  FETCH_RATES_SUCCESS
} from "./rates.constants";
import { queryStringBuilder } from "../../utils";
import { RatesDispatch } from "./rates.model";

export const fetchRatesRequest = () => ({
  type: FETCH_RATES_REQUEST
});

export const fetchRatesSuccess = rates => ({
  type: FETCH_RATES_SUCCESS,
  payload: rates
});

export const fetchRatesFailure = error => ({
  type: FETCH_RATES_FAILURE,
  payload: error
});

export const handle500Error = () => ({
  type: HTTP_500_ERROR
});

const setTrend = (prev: number, cur: number) => {
  if (cur > prev) {
    return "growing";
  } else if (cur < prev) {
    return "declining";
  } else if (cur === prev) {
    return "stagnating";
  }
};

export const fetchRatesAsync = () => {
  return async (dispatch: RatesDispatch, getState) => {
    dispatch(fetchRatesRequest());
    try {
      const { currencies } = getState().configuration;
      const { ratesList } = getState().rates;
      const ids = Object.keys(currencies);
      const qs = queryStringBuilder(ids);
      const response = await fetch(`http://localhost:3000/rates?${qs}`);
      if (response.status === 500) {
        dispatch(handle500Error());
      }
      const data = await response.json();
      const { rates } = await data;
      let trend;
      const formattedRates = ids.map(id => {
        const rate = rates[id];
        const previousRate = ratesList[id] && ratesList[id].rate;
        ratesList[id] !== undefined
          ? (trend = setTrend(previousRate, rate))
          : (trend = "N/A");
        return {
          rate,
          trend
        };
      });
      dispatch(fetchRatesSuccess(formattedRates));
    } catch (err) {
      dispatch(fetchRatesFailure(err));
    }
  };
};
