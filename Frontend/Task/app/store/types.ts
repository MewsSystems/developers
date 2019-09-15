import { AlertMessagesProps } from "@components/ui/AlertMessages";

export interface ApplicationState {
    currencyState: CurrencyState,
    ratesState: RatesState,
    alert: AlertMessagesProps
}

export interface CurrencyState {
    loading: boolean,
    currencyPairs: StringTMap<CurrencyPair>,
    currencyPairsIds: string[],
}

export interface RatesState {
    rates: StringTMap<Rate>
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