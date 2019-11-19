import axios from 'axios';

const LOCAL = 'localhost:3000';

const baseURL = `http://${LOCAL}/`;

const Connector = function(params = {}) {
  this.base = baseURL;
  this.params = params;
};

Connector.prototype.parameters = function(params) {
  return new Connector({ ...this.params, params });
};

Connector.prototype.get = function(url) {
  return new Connector({ ...this.params, method: 'GET', url });
};

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
        return resolve(response, response.data)
      })
      .catch((response) => {
        return reject(response);
      })
  });
};

export default new Connector();
