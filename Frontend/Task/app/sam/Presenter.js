import { render } from "./Renderer";
import { State } from "./State";
import { updateCurrencyPairValues, decrementCounter, fetchCurrencyPairs, loadConfiguration } from "./Actions";
import { CONTROL_STATES } from "./Model";

export const nextAction = model => {
    let result = false;
    switch (model.control) {
        case CONTROL_STATES.FIRST_FETCH:
            result = true;
        case CONTROL_STATES.TIME_TO_UPDATE:
            updateCurrencyPairValues(
                model.endpoint,
                Array.from(Object.keys(model.currencyPairsById))
            );
            break;
        case CONTROL_STATES.DECREMENT:
            decrementCounter();
            break;
        case CONTROL_STATES.GET_CONFIGURATION:
            fetchCurrencyPairs();
            break;
        case CONTROL_STATES.GET_CONFIGURATION_FROM_DB:
            loadConfiguration();
            break;
        case CONTROL_STATES.NO_ACTION:
        default:
            break;
    }
    return result;
};

export const present = model => {
    const state = new State();
    if (!nextAction(model)) {
        const pairs = Object.entries(model.currencyPairsById);
        if (pairs.length === 0) {
            state.message = "Loading...";
        } else if (model.control === CONTROL_STATES.TIME_TO_UPDATE) {
            state.message = "Updating...";
        } else {
            if (!model.lastFailed) {
                state.message = `Next update in ${model.counter} secs`;
            } else {
                state.message = `Last update failed. Next in ${model.counter} secs`;
            }
        }
        state.pairs = pairs.map(([_, pair]) => pair.clone());
        state.selected = state.pairs.filter(pair => pair.selected);
        render(state);
    }
};
