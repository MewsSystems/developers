export enum UserAction {
    addRate = 'add-rate',
    removeRate = 'remove-rate'
}

export enum AppAction {
    toggleLoading = 'toggle-loading',
    setRates = 'set-rates',
    getRates = 'get-rates',
    getConfig = 'get-config'
}

export interface ReducerAction<T> {
    type: T;
    data?: any;
}
