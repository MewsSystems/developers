export enum RateType {
    'growing' = 'growing',
    'declining' = 'declining',
    'stagnating' = 'stagnating'
}

export interface CurrencyPair {
    code: string;
    name: string;
}

export interface CurrencyList {
    currencyPairs: CurrencyObject;
}

export interface CurrencyObject {
    [index: string]: CurrencyPair[];
}

export interface RatesList {
    rates: RatesObject;
}

export interface RatesObject {
    [index: string]: number;
}

export enum Storage {
    'currentRates' = 'current-rates',
    'currentConfig' = 'current-config'
}

export interface ParsedCurrency {
  id: string;
  currency1: CurrencyPair;
  currency2: CurrencyPair;
}

export interface ParsedRate {
    id: string;
    name: string;
    value: number;
    type?: RateType;
}