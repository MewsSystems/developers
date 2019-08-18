import { FETCH_CONFIGURATION, FETCH_RATES, REQSUEST_FAILED, TOGGLE_FILTER } from './actions';

const initialState = {
    configuration : {
        isLoading: true,
        currencyPairs : []
    },
    rates : {
        isLoading: true,
        currencyPairsRates: []
    },
    filter: [],
    error: false
}

const reducer = (state = initialState, action) => {
    switch (action.type) {
        case FETCH_CONFIGURATION:
            return {
                ...state,
                configuration : {
                    isLoading: false,
                    currencyPairs : action.currencyPairs
                },
                error: false
            }
        case FETCH_RATES:
            let Rates = [];
            Object.keys(action.rates).map(key => {
                Rates[key] = [];
                Rates[key].status = state.rates.currencyPairsRates[key] ? action.rates[key] - state.rates.currencyPairsRates[key].value: 0;
                Rates[key].value = action.rates[key];
                return true;
            });
            return {
                ...state,
                rates : {
                    isLoading: false,
                    currencyPairsRates: Rates
                },            
                error: false
            }
        case REQSUEST_FAILED:
            return {
                ...state,
                error: true
            }
        case TOGGLE_FILTER:
            let tmpFilter = state.filter.slice();
            if(!action.payload.isChecked){
                return {
                    ...state,
                    filter: tmpFilter.filter(id => id !== action.payload.pairsID)
                } 
            }else{
                tmpFilter.splice(tmpFilter.length, 0, action.payload.pairsID)
                return {
                    ...state,
                    filter: tmpFilter
                }
            }
        default:
            return state;
      }      
};

export default reducer;