import axios from 'axios';

import config from '../config';

const baseURL = `http://${config.server}/`;

/**
 * Connector for axios requests
 * @param {Object} params
 * @constructor
 */
const Connector = function(params = {}) {
  this.base = baseURL;
  this.params = params;
};

/**
 * Collect params for GET request
 * @param {Object} params
 * @return {Connector}
 */
Connector.prototype.parameters = function(params) {
  return new Connector({ ...this.params, params });
};

/**
 * Setting GET method for request
 * @param {string} url
 * @return {Connector}
 */
Connector.prototype.get = function(url) {
  return new Connector({ ...this.params, method: 'GET', url });
};

/**
 * Send axios request to api
 * @return {Promise<>}
 */
Connector.prototype.send = function() {
  return new Promise((resolve, reject) => {
    const params = {
      ...this.params,
      baseURL: this.base,
      method: this.method,
      url: this.params.url,
    };

    axios(params)
      .then((response) => {
        return resolve(response, response.data);
      })
      .catch((response) => {
        return reject(response);
      })
  });
};

export default new Connector();
