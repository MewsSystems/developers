const SERVER_ENDPOINT = 'http://localhost:3000';

export const getConfigurationUrl = () => {
    return SERVER_ENDPOINT + '/configuration';
};

export const getPairsUrl = (ids) => {
    return SERVER_ENDPOINT + '/rates?' + prepareParams(ids);
};

const prepareParams = (object) => {
    let parameters = [];
    for (let key in object) {
        if (object.hasOwnProperty(key)) {
            parameters.push(encodeURI( 'currencyPairIds[]=' + object[key]));
        }
    }

    return parameters.join('&');
};