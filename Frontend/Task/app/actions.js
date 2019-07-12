export const getConfiguration = () => {
	return { type: 'GET_CONFIGURATION' }
};

export const configurationSucceeded = (data = {}) => {
	console.log('data', data);
	return { type: 'CONFIGURATION_SUCCEEDED', data: data }
};

export const getRate = () => {
	return { type: 'GET_RATE' }
};

export const rateSucceeded = (data = {}) => {
	return { type: 'RATE_SUCCEEDED', data }
};


export const selectCurrency = (data = []) => {
	return { type: 'SELECT_CURRENCY', data }
};
