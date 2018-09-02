import { combineReducers } from 'redux';
import {
    configReducer as config,
    currencyPairsReducer as currencyPairs,
    errorMessagesReducer as errorMessages,
    isFetchingReducer as isFetching,
    ratesHistoryReducers as ratesHistory,
    uiControlReducer as uiControl,
} from '../reducers';

const rootReducer = combineReducers({
    config,
    currencyPairs,
    errorMessages,
    isFetching,
    ratesHistory,
    uiControl,
});

export default rootReducer;
