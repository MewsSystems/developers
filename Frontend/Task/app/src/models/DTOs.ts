import Currency from "./Currency";

export type CurrencyPairConfigDTO = Record<
  string,
  Currency[]
>; /**Object from /configuration => id -> pair of currencies */

export type RatesDTO = Record<string, Number>; /**Rates - id -> rate */

export type LocalStorageDTO = {
  [id: string]: {
    currencies: Currency[];
    shown: Boolean;
  };
};
