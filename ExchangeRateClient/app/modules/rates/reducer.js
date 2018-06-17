import _ from 'lodash';
import {FETCH_CURRENCY_PAIRS_SUCCEEDED} from './actions';

const INITIAL_STATE = {
    /*
    currencyPairs: {
        // {"70c6744c-cba2-5f4c-8a06-0dac0c4e43a1":[{"code":"AMD","name":"Armenia Dram"},{"code":"GEL","name":"Georgia Lari"}],

        // { [id]: { id: id, pair: [] } }
    }*/
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
        default:
            return state;
    }
}
