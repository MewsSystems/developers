import {AppAction, ReducerAction} from "../../types/actions";
import {CurrencyList, RatesObject} from "../../types/app";

export const toggleProgress = (status: boolean): ReducerAction<AppAction> => {
    return {
        type: AppAction.toggleLoading,
        data: status
    };
};

export const getConfig = () => dispatch => {
    return new Promise<any>(resolve => {
        dispatch(toggleProgress(true));

        fetch(`${process.env.SERVER_URL}/configuration`)
            .then(res => res.json())
            .then((currencies: CurrencyList) => {
                dispatch({
                    type: AppAction.getConfig,
                    data: currencies.currencyPairs
                });
                dispatch(toggleProgress(false));
                resolve();
            });
    });
};

export const getRates = (rates: RatesObject) => {
    return {
        type: AppAction.getRates,
        data: rates
    }
};

export const setError = (state: boolean) => {
   return {
       type: AppAction.toggleError,
       data: state
   };
};

export const setRateCallTime = (time: string) => {
  return {
      type: AppAction.setRateCallTime,
      data: time
  };
};

export const clearRates = () => {
    return {
        type: AppAction.clearRates
    };
};
