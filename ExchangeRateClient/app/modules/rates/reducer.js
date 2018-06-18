import {FETCH_CURRENCY_PAIRS_SUCCEEDED} from './actions';
import {FETCH_CURRENCY_RATES_SUCCEEDED} from './actions';

const INITIAL_STATE = {
};

export default (state = INITIAL_STATE, action) => {
    switch (action.type) {
        case FETCH_CURRENCY_PAIRS_SUCCEEDED: {
            const {currencyPairs} = action.payload;

            // let newArr = [];
            // for (let key in currencyPairs) {
            //     if (currencyPairs.hasOwnProperty(key)) {
            //         let newObj = {
            //             id: key,
            //             currency1: currencyPairs[key][0],
            //             currency2: currencyPairs[key][1]
            //         };
            //         newArr.push(newObj);
            //     }
            // }

            return {
                ...state,
                // currencyPairs: newArr
                currencyPairs: currencyPairs
            };
        }
        case FETCH_CURRENCY_RATES_SUCCEEDED: {
            const {rates} = action.payload;

            let newState = JSON.parse(JSON.stringify(state)); //dirty hack

            for (let key in rates) {
                if (rates.hasOwnProperty(key) && newState.currencyPairs.hasOwnProperty(key)) {
                    if (newState.currencyPairs[key].length <= 2) {
                        newState.currencyPairs[key].push({rate: rates[key]});
                    } else {
                        newState.currencyPairs[key][2] = {rate: rates[key]};
                    }
                }
            }

            return newState;
        }
        default:
            return state;
    }
}
