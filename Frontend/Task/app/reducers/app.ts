import {AppState} from "../../types/state";
import {initialStore} from "../common/constants";
import {AppAction, ReducerAction} from "../../types/actions";

export default (state: AppState = initialStore.app, action: ReducerAction<AppAction>) => {
    switch (action.type) {
        case AppAction.toggleLoading:
            return {...state, loading: action.data};
        case AppAction.getConfig:
            return {...state, currencies: action.data};
        case AppAction.getRates:
            return {...state, rates: action.data};
        case AppAction.setRateCallTime:
            return {...state, date: action.data};
        case AppAction.toggleError:
            return {...state, error: action.data};
    }

    return state;
};