import { Dispatch } from "redux";

export interface Currency {
    code: string,
    name: string
}

export type CurrencyPair = {
    [0]: Currency,
    [1]: Currency
}

export interface KeyByCurrencyPair {
    [id: string]: CurrencyPair
}

export interface ConfigurationState {
    [key: string]: any,
    loading: boolean,
    currencyPairs: KeyByCurrencyPair[]
    currencyPairsIdList: string[]
}

export interface ApplicationState {
    configuration: ConfigurationState
}

export interface ConnectedReduxProps {
    dispatch: Dispatch
}