import {FETCH_CURRENCY_PAIRS_SUCCEEDED} from './actions';
import {FETCH_CURRENCY_RATES_SUCCEEDED} from './actions';

const INITIAL_STATE = {
};

export default (state = INITIAL_STATE, action) => {
    switch (action.type) {
        case FETCH_CURRENCY_PAIRS_SUCCEEDED: {
            const {currencyPairs} = action.payload;

            let newArr = [];
            for (let key in currencyPairs) {
                if (currencyPairs.hasOwnProperty(key)) {
                    let newObj = {
                        id: key,
                        currency1: currencyPairs[key][0],
                        currency2: currencyPairs[key][1]
                    };
                    newArr.push(newObj);
                }
            }

            return {
                ...state,
                currencyPairs: newArr
            };
        }
        case FETCH_CURRENCY_RATES_SUCCEEDED: {
            const {rates} = action.payload;

            return {
                ...state,
                rates
            };
        }
        default:
            return state;
    }
}
