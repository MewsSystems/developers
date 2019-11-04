import { createStore, applyMiddleware, combineReducers } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { reducer as formReducer } from 'redux-form';
import { combineEpics, createEpicMiddleware } from 'redux-observable';
import currencyPairsReducer, { fetchCurrencyPairsEpic } from './ducks/currencyPairs';
import currencyPairsRatesReducer, { fetchCurrencyPairsRatesEpic } from './ducks/currencyPairsRates';

const epicMiddleware = createEpicMiddleware();

const rootEpic = combineEpics(
    fetchCurrencyPairsEpic,
    fetchCurrencyPairsRatesEpic,
);

const rootReducer = combineReducers({
    // ...your other reducers here
    form: formReducer,
    currencyPairs: currencyPairsReducer,
    currencyPairsRates: currencyPairsRatesReducer,
});

export default function configureStore() {
    const store = createStore(rootReducer, composeWithDevTools(
        applyMiddleware(epicMiddleware),
        // other store enhancers if any
    ));

    epicMiddleware.run(rootEpic);

    return store;
}
