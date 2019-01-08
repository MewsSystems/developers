import {server} from '../../src/config';

export default (route, callback, query = {}) => {
  const q = Object.keys (query)
    .map (k => `${encodeURIComponent (k)}=${encodeURIComponent (query[k])}`)
    .join ('&'); // constructing a query

  fetch (server + route + '?' + q)
    .then (function (response) {
      return response.json ();
    })
    .then (function (res) {
      callback && callback (null, res);
    })
    .catch (function (err) {
      callback && callback (err, null);
    });
};
