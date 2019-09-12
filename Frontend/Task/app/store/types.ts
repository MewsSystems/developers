import { Dispatch } from "redux";

interface Currerncy {
    code: string,
    name: string
}

export interface CurrencyPair {
    [id: number]: Currerncy
}

export interface ConfigurationState {
    [key: string]: any,
    loading: boolean,
    currencyPairs: CurrencyPair[]
}

export interface ApplicationState {
    configuration: ConfigurationState
}

export interface ConnectedReduxProps {
    dispatch: Dispatch
}