const initialState = {
	configuration: {},
	currencyPairsRateList: [],
	pairsSelector: [],
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
		default:
			return state;
	}

	// switch (action.type) {
	// 	case 'REQUESTED_DOG':
	// 		return {
	// 			url: '',
	// 			loading: true,
	// 			error: false,
	// 		};
	// 	case 'REQUESTED_DOG_SUCCEEDED':
	// 		return {
	// 			url: action.url,
	// 			loading: false,
	// 			error: false,
	// 		};
	// 	case 'REQUESTED_DOG_FAILED':
	// 		return {
	// 			url: '',
	// 			loading: false,
	// 			error: true,
	// 		};
	// 	default:
	// 		return state;
	// }
};

export default reducer;
