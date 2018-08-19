import {
  GET_CURRENCY_PAIRS_LOADING,
  GET_CURRENCY_PAIRS_SUCCESS,
  GET_CURRENCY_PAIRS_ERROR
} from "./types";

import { endpoint } from "../config";
import axios from "axios";

const getCurrencyPairsLoading = () => ({
  type: GET_CURRENCY_PAIRS_LOADING
});

const getCurrencyPairsSuccess = ({ currencyPairs }) => ({
  type: GET_CURRENCY_PAIRS_SUCCESS,
  currencyPairs
});

const getCurrencyPairsError = error => ({
  type: GET_CURRENCY_PAIRS_ERROR,
  error
});

export const getPairs = () => dispatch => {
  dispatch(getCurrencyPairsLoading());

  return axios({
    url: `${endpoint}/configuration`,
    method: "get"
  })
    .then(res => {
      dispatch(getCurrencyPairsSuccess(res.data));
      return res;
    })
    .catch(error => {
      dispatch(getCurrencyPairsError(error));
      return error;
    });
};
