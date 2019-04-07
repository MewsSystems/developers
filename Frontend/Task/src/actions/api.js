import 'whatwg-fetch';
import isObject from 'lodash.isobject';

/**
 * Fetch API wrapper
 */
export function fetchApiNative(url, method, parameters) {
  let fetchUrl = url;
  let fetchParams = { method: method || 'GET' };

  const params = parameters ? Object.assign({}, parameters) : {};

  if (fetchParams.method === 'GET') {
    fetchUrl = Object.keys(params)
      .reduce((previousValue, currentValue) => {
        if (Object.prototype.hasOwnProperty.call(params, currentValue)) {
          return `${previousValue}${currentValue}=${encodeURIComponent(params[currentValue])}&`;
        }
        return previousValue;
      }, `${fetchUrl}?`);
    fetchUrl = fetchUrl.substring(0, fetchUrl.length - 1);
    fetchParams.headers = {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    };
  } else if (params instanceof FormData) {
    fetchParams = { method: 'post', body: params };
  } else {
    fetchParams.body = JSON.stringify(params);
    fetchParams.headers = {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    };
  }

  fetchParams.credentials = 'same-origin';

  return fetch(fetchUrl, fetchParams)
    .then(response => {
      const resContentType = response.headers.get('Content-Type');

      if (response.ok && response.status === 200) {
        if (/text\/plain/.test(resContentType)) return response.text();
        if (/application\/json/.test(resContentType)) return response.json();
        return response.body;
      }

      const error = {
        status: response.status || 500,
        message: response.statusText,
        statusText: response.statusText,
        response,
      };

      if (/application\/json/.test(resContentType)) {
        return response.json()
          .then(jsonResult => {
            error.message = jsonResult.message || response.statusText;
            return Promise.resolve(
              { response: { ...jsonResult, statusText: response.statusText }, error },
            );
          });
      }
      return Promise.reject(error);
    })
    .then(response => {
      if (response.error) {
        const error = {
          status: response.error.status,
          statusText: response.error.statusText,
          message: response.error.message,
          response: response.response,
        };
        return Promise.reject(error);
      }

      if (!isObject(response)) return { value: response };
      return response;
    })
    .catch(error => {
      const errorObj = error;
      if ((error.status && error.status.toString()[0] === 5)) {
        errorObj.message = 'Sorry, server is unavailable. Please, try again later';
      }
      return Promise.reject(errorObj);
    });
}

/**
 * Fetch wrapper
 */
export function fetchApi(url, action, method = 'GET', params, options) {
  let reactAction = {};
  if (typeof action === 'string') {
    reactAction.type = action;
  } else {
    reactAction = { ...action };
  }

  return (dispatch) => {
    dispatch({ ...reactAction, type: `REQUEST_${reactAction.type}` });

    return fetchApiNative(url, method, params, options)
      .then(
        json => {
          dispatch({ ...reactAction, type: `SUCCESS_${reactAction.type}`, payload: json });
          return json;
        },
        error => {
          dispatch({ ...reactAction, type: `ERROR_${reactAction.type}`, payload: error });
          return Promise.reject(error);
        },
      );
  };
}
