import {AppState, UserState} from "../../types/state";
import {initialStore} from "../common/constants";
import {AppAction, ReducerAction, UserAction} from "../../types/actions";

export default (state: UserState = initialStore.user, action: ReducerAction<UserAction>) => {
    switch (action.type) {
        case UserAction.addRate:
            let rates = state.userRates.slice(0);
            rates.push(action.data);

            return {...state, userRates: rates};
        case UserAction.removeRate:
            let filteredRates = state.userRates.slice(0);

            return {...state, userRates: filteredRates.filter(rate => rate !== action.data)};
    }

    return state;
};