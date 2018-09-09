import { GET_RATES_LOADING, GET_RATES_SUCCESS, GET_RATES_ERROR } from './types';

export const getRatesLoading = () => ({
  type: GET_RATES_LOADING
});

export const getRatesSuccess = rates => ({
  type: GET_RATES_SUCCESS,
  rates
});

export const getRatesError = error => ({
  type: GET_RATES_ERROR,
  error
});
