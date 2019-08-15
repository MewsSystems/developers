import {AppAction, ReducerAction} from "../../types/actions";
import {CurrencyList} from "../../types/app";

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