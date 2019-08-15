import {CurrencyObject, RatesObject} from "./app";

export interface RootState {
    app: AppState;
    user: UserState;
}

export interface AppState {
    loading: boolean;
    rates: RatesObject;
    currencies: CurrencyObject;
}

export interface UserState {
    userRates: string[];
}