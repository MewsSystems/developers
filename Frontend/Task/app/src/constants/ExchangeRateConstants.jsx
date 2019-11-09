const SERVER_ENDPOINT = 'http://localhost:3000';

export const getConfigurationUrl = () => {
    return SERVER_ENDPOINT + '/configuration';
};

export const getPairUrl = (code) => {
    return SERVER_ENDPOINT + '/rates?currencyPairIds[]=' + code;
};