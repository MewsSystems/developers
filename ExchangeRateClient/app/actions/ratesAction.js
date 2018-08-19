import { endpoint } from "../config";
import { GET_RATES_LOADING, GET_RATES_SUCCESS, GET_RATES_ERROR } from "./types";
import axios from "axios";

const getRatesLoading = () => ({
  type: GET_RATES_LOADING
});

const getRatesSuccess = rates => ({
  type: GET_RATES_SUCCESS,
  rates
});

const getRatesError = error => ({
  type: GET_RATES_ERROR,
  error
});

export const getRates = array => dispatch => {
  dispatch(getRatesSuccess());
  let urlQuery = "?currencyPairIds[]=" + array.join("&currencyPairIds[]=");
  console.log(urlQuery);
  return axios({
    url: `${endpoint}/rates${urlQuery}`,
    method: "request"
  })
    .then(res => {
      dispatch(getRatesSuccess(res.data));
      return res;
    })
    .catch(error => {
      dispatch(getRatesError(error));
      return error;
    });
};
