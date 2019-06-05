import { ThunkDispatch } from "redux-thunk";

export interface CurrencyPair {
  code: string;
  name: string;
}

export interface CurrencyPairs {
  currencyPairs: {
    [key: string]: CurrencyPair[];
  };
}

export interface CurrencyPairIds {
  currencyPairIds: string[];
}

export interface Rates {
  rates: {
    [id: string]: number;
  };
}

export type Action =
  | { type: "FETCH_EXCHANGE" }
  | { type: "FETCH_EXCHANGE_PENDING" }
  | { type: "FETCH_EXCHANGE_FULFILLED"; payload: { rates: Rates } }
  | { type: "FETCH_EXCHANGE_REJECTED" }
  | { type: "DOWNLOAD_CONFIG" }
  | { type: "DOWNLOAD_CONFIG_PENDING" }
  | {
      type: "DOWNLOAD_CONFIG_FULFILLED";
      payload: { currencyPairs: CurrencyPairs };
    }
  | { type: "DOWNLOAD_CONFIG_REJECTED" };

export interface AppState {
  rates?: Rates;
  isFetching?: boolean;
  currencyPairs?: CurrencyPairs;
  error?: string;
}

export type ExchangeThunkDispatch = ThunkDispatch<AppState, undefined, Action>;
