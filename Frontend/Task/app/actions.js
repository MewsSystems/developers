export const getConfiguration = () => {
	return { type: 'GET_CONFIGURATION' }
};

export const configurationSucceeded = (data = {}) => {
	return { type: 'CONFIGURATION_SUCCEEDED', data: data.currencyPairs || {} }
};

export const getRate = () => {
	return { type: 'GET_RATE' }
};

export const rateSucceeded = (data = {}) => {
	return { type: 'RATE_SUCCEEDED', data }
};

