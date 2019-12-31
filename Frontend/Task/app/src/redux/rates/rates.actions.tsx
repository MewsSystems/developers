import "core-js/stable";
import "regenerator-runtime/runtime";
import {
  FETCH_RATES_FAILURE,
  FETCH_RATES_RETRY,
  FETCH_RATES_REQUEST,
  FETCH_RATES_SUCCESS}
from './rates.constants'
import {queryStringBuilder, setTrend} from '../../utils'

export const fetchRatesRequest = () => ({
  type: FETCH_RATES_REQUEST
})

export const fetchRatesSuccess = (rates) => ({
  type: FETCH_RATES_SUCCESS,
  payload: rates,
})

export const fetchRatesFailure = (error) => ({
  type: FETCH_RATES_FAILURE,
  payload: error
})

export const fetchRatesRetry = (rates) => ({
  type: FETCH_RATES_RETRY,
  payload: rates
})


export const fetchRatesAsync = () => {
  return async (dispatch, getState) => {
    dispatch(fetchRatesRequest())
    try {
      const {currencies} = getState().configuration;
      const {ratesList} = getState().rates
      const ids = Object.keys(currencies)
      const qs = queryStringBuilder(ids)
      const response = await fetch(`http://localhost:3000/rates?${qs}`)
      const data = await response.json();
      const {rates} = await data;
      let newRates = {};
      let trend;
      ids.map(id => {
        const currentRate = rates[id];
        const previousRate = ratesList[id] && ratesList[id].currentRate;
        ratesList[id] !== undefined ? (trend = setTrend(previousRate, currentRate)): (trend = "N/A")
        return newRates[id] = {
          currentRate,
          previousRate,
          trend
        }
      })
      dispatch(fetchRatesSuccess(newRates))
    } catch(err) {
      dispatch(fetchRatesFailure(err))
    }
  }
}
