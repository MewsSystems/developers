import * as types from '../actions/types';
import { set, get,compareRates} from '../utils/index';

const configuration = get('configuration');

const initialState = {
  configuration: configuration,
	rates: {},
	isLoadingConfiguration:true,
	isLoadingRates:true,
	status:200,
	request:''
};

const CurrencyReducer = (state = initialState, action) => {
  switch (action.type) {
    case types.GET_CONFIGURATION_SUCCESS:
		set('configuration', action.configuration);
      return {
        ...state,
        configuration: action.configuration,
				isLoadingConfiguration:false,
				status:200
      };
			case types.GET_RATE_SUCCESS:

      let mappedRates = {} ;

      Object.keys(action.rates).map(key => {
        return mappedRates[key] = {
          name: state.configuration[key] ? state.configuration[key][0].code + ' - ' + state.configuration[key][1].code : 0,
          value: action.rates[key]
        };
      });

			let oldRates = get('rates');
			set('rates',mappedRates);

			let oldRatesArray = Object.values(oldRates);
			let newRatesArray = Object.values(mappedRates);

			compareRates(oldRatesArray,newRatesArray);

			return {
				...state,
				rates: mappedRates,
				isLoadingRates:false,
				status:200
			};
				case types.RESPONSE_ERROR:
		      return {
		        ...state,
		        status: action.status,
		      };
					case types.REQUEST_ERROR:
			      return {
			        ...state,
			        request: action.request,
			      };
    default:
      return state;
  }
};

export default CurrencyReducer;
