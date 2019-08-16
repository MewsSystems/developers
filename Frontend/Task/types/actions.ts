export enum UserAction {
    addRate = 'add-rate',
    removeRate = 'remove-rate'
}

export enum AppAction {
    toggleLoading = 'toggle-loading',
    getRates = 'get-rates',
    getConfig = 'get-config',
    clearRates = 'clear-rates',
    toggleError = 'toggle-error',
    setRateCallTime = 'set-rate-call-time'
}

export interface ReducerAction<T> {
    type: T;
    data?: any;
}
