import {AppAction, UserAction} from "../../types/actions";
import {RootState} from "../../types/state";
import {appConfig, userRates} from "../common/storage";

export default api => next => action => {
    const state: RootState = api.getState();

    switch (action.type) {
        case AppAction.getConfig:
            appConfig.set(action.data);
            break;
        case UserAction.addRate:
            let rates = state.user.userRates.slice(0);
            rates.push(action.data);

            userRates.set(rates);

            action.data = rates;
            break;
        case UserAction.removeRate:
            let filteredRates = state.user.userRates.filter(rate => rate !== action.data);

            userRates.set(filteredRates);

            action.data = filteredRates;
            break;
    }

    return next(action);
};