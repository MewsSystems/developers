import { FETCH_CONFIGURATION, FETCH_RATES, REQSUEST_FAILED, TOGGLE_FILTER } from './actions';
import { saveToLocalStorage, loadFromLocalStorage, FILTER, CURRENCY_PAIRS} from './localStorage';
import IRootState from '../interfaces/InitialState.interface'
import ICurrencyPairsRates from '../interfaces/CurrencyPairsRates.interface'

const currencyPairs = loadFromLocalStorage(CURRENCY_PAIRS);

const initialState: IRootState = {
  configuration : {
    isLoading: currencyPairs.length > -1 ? true : false,
    currencyPairs : currencyPairs,
  },
  rates : {
    isLoading: true,
    currencyPairsRates: {}
  },
  filter: loadFromLocalStorage(FILTER),
  error: false
}

const reducer = (state = initialState, action: any) => {
  switch (action.type) {
    case FETCH_CONFIGURATION:
      saveToLocalStorage(CURRENCY_PAIRS, action.currencyPairs);
      return {
        ...state,
        configuration : {
          isLoading: false,
          currencyPairs : action.currencyPairs
        },
        error: false
      }
    case FETCH_RATES:
      let formatedRates: ICurrencyPairsRates = {};
      Object.keys(action.rates).map(key => {
        return formatedRates[key] = {
          status: state.rates.currencyPairsRates[key] ? action.rates[key] - state.rates.currencyPairsRates[key].value: 0,
          value: action.rates[key]
        };
      });
      return {
        ...state,
        rates : {
          isLoading: false,
          currencyPairsRates: formatedRates
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
        tmpFilter = tmpFilter.filter(id => id !== action.payload.pairsID);
      }else{
        tmpFilter.splice(tmpFilter.length, 0, action.payload.pairsID);
      }
      saveToLocalStorage(FILTER, tmpFilter);
      return {
        ...state,
        filter: tmpFilter
      }
    default:
      return state;
  }      
};

export default reducer;