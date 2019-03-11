import { CurrencyInterface } from './currencyInterface';

export enum ExchangeAction {
    'GET_PAIRS',
    'SET_SELECTED',
    'LOADING_TOGGLE',
    'ERROR_TOGGLE',
    'ADD_MESSAGE',
}

export interface GetPairsActionInterface {
    type: typeof ExchangeAction.GET_PAIRS;
    payload: {
        [pairId: string]: {pair: CurrencyInterface[]};
    };
}

export interface ToggleInterface {
    type: typeof ExchangeAction.ERROR_TOGGLE | ExchangeAction.LOADING_TOGGLE;
    payload?: boolean;
}

export interface SetSelectedInterface {
    type: typeof ExchangeAction.SET_SELECTED;
    payload: string;
}

export interface AddMessageInterface {
    type: typeof ExchangeAction.ADD_MESSAGE;
    payload: {
        message: string;
        type: string;
    };
}

export type ExchangeActionTypes =
    GetPairsActionInterface | ToggleInterface | SetSelectedInterface | AddMessageInterface;
