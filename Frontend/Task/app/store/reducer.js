import { currencyLocalDB } from '../localDb';

const initialState = {
	configuration: currencyLocalDB.get('currency_configuration_item') || {},
	currencyPairsRateList: [],
	pairsSelector:  currencyLocalDB.get('currency_user_selection') || [],
};

const reducer = (state = initialState, action) => {
	switch (action.type) {
		case 'GET_CONFIGURATION':
			return {
				...state
			};
		case 'CONFIGURATION_SUCCEEDED':
			return {
				...state,
				configuration: action.data
			};
		case 'GET_RATE':
			return {
				...state
			};
		case 'RATE_SUCCEEDED':
			return {
				...state,
				currencyPairsRateList: action.data
			};
		case 'SELECT_CURRENCY':
			return {
				...state,
				pairsSelector: action.data
			};
		default:
			return state;
	}
};

export default reducer;
