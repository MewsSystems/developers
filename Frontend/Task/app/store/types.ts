
export interface ApplicationState {
    loading: boolean,
    currencyPairs: StringTMap<CurrencyPair>,
    currencyPairsIds: string[],
    rates: StringTMap<Rate>,
}

export interface KeyByCurrencyPair {
    [key: string]: CurrencyPair
}

export type CurrencyPair = {
    [0]: Currency,
    [1]: Currency
}

export interface Currency {
    code: string,
    name: string
}

export interface Rate {
    value: number,
    trend: Trend
}

export type Trend = "growing" | "declining" | "stagnating";

export interface StringTMap<T> { [key: string]: T; };