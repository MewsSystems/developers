import { combineReducers } from 'redux';
import { reducer as productsReducer, MODULE_NAME as productsKey } from './products';

export default combineReducers({
    [productsKey]: productsReducer,
});
