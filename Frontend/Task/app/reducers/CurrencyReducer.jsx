import * as types from '../actions/types';
import {
 set, get, checkTrend, parseDate,
} from '../utils/index';

const configuration = get('configuration');

const initialState = {
  configuration,
	rates: [],
	isLoadingConfiguration: true,
	isLoadingRates: true,
	status: 200,
	request: '',
	timestamp: '',
	rateId: get('rateId'),
};

const CurrencyReducer = (state = initialState, action) => {
  switch (action.type) {
    case types.GET_CONFIGURATION_SUCCESS: {
			set('configuration', action.configuration);
			return {
				...state,
				configuration: action.configuration,
				isLoadingConfiguration: false,
				status: 200,
			};
		}
			case types.GET_RATE_SUCCESS: {
				let mappedRates = [];
				const oldRates = get('rates');

				Object.keys(action.rates).map((key) => {
					let oldRateItem = oldRates.find((entry) => entry.id === key);
					if (typeof oldRateItem === 'undefined') {
							oldRateItem = { value: 0 };
					}
					mappedRates.push({
						id: key,
	          name: `${state.configuration[key][0].code} / ${state.configuration[key][1].code}`,
	          value: action.rates[key],
						oldValue: oldRateItem.value,
						type: checkTrend(oldRateItem.value, action.rates[key]),
           });
        });
				set('rates', mappedRates);

				return {
					...state,
					rates: mappedRates,
					isLoadingRates: false,
					status: 200,
					timestamp: parseDate(new Date()),
				};
			}
			case types.FILTER: {
				set('rateId', action.rateId);
				return {
					...state,
					rateId: action.rateId,
				};
			}
				case types.RESPONSE_ERROR: {
					return {
						...state,
						status: action.status,
					};
				}
				case types.REQUEST_ERROR: {
					return {
						...state,
						request: action.request,
					};
				}

    default:
      return state;
  }
};

export default CurrencyReducer;
