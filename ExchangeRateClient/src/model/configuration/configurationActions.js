import config from '../../config';

export const SET_CONFIG = 'SET_CONFIG';

export function setConfig(data) {
    return {
        type: SET_CONFIG,
        data,
    };
}

export function loadConfig(data) {
    return dispatch => {
        fetch(`${config.apiUrl}/configuration`, {method: 'GET'})
            .then(response => response.json())
            .then(data => {
                dispatch(setConfig(data));
            });
    }
}
