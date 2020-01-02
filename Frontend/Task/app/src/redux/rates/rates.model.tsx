import {
  FETCH_RATES_FAILURE,
  FETCH_RATES_SUCCESS,
  FETCH_RATES_REQUEST,
  HTTP_500_ERROR
} from "./rates.constants";
import { ThunkDispatch } from "redux-thunk";
import { Action } from "redux";

export interface IFetchRatesRequest {
  type: typeof FETCH_RATES_REQUEST;
}

export interface IFetchRatesSuccess {
  type: typeof FETCH_RATES_SUCCESS;
  payload: RatesData;
}

export interface IFetchRatesFailure {
  type: typeof FETCH_RATES_FAILURE;
  payload: string;
}

export interface IHTTP500Error {
  type: typeof HTTP_500_ERROR;
}
export interface RatesData {
  rates?: IRate;
}

export interface IRate {
  rate: number;
  trend: "N/A" | "growing" | "declining" | "stagnating";
}

export interface RateReducerState {
  rates: {
    ratesList?: RatesData;
    isLoading: boolean;
    error: string;
  };
}

export interface RatesState {
  ratesList?: RatesData;
  isLoading: boolean;
  errorMessage: string;
  showErrorAlert: boolean;
}

export type RatesAction =
  | IFetchRatesRequest
  | IFetchRatesSuccess
  | IFetchRatesFailure
  | IHTTP500Error;

export type RatesDispatch = ThunkDispatch<RatesData, void, Action>;
