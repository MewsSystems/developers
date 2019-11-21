import {server as API_URL} from '../../src/config';
import {stringifyQuery} from '../utils';

const apiCall = async (...args) => {
  const [route, query, params = {}] = args;

  const {
    retry = true, // by default any request will attempt to retry
    timeout = 300, // time to wait until retrying fetch
  } = params;

  const url = getApiUrl (route, query);

  try {
    return await fetch (url).then (response => response.json ());
  } catch (error) {
    if (!retry) throw error;

    if (process.env.NODE_ENV !== 'production') {
      console.warn (`fetch failed, retry in ${timeout}ms, URL: ${url}`);
    }

    await wait (timeout);

    return await apiCall (...args);
  }
};

export default apiCall;

function getApiUrl (route, queryObject) {
  let query = stringifyQuery (queryObject);
  const endpoint = `${API_URL}${route}`;
  const hasQuery = queryObject && query.trim ().length > 0;
  if (hasQuery) query = '?' + query;
  return `${endpoint}${query}`;
}

const wait = async (timeout = 0) => {
  if (timeout === 0) return;
  await new Promise (r => setTimeout (r, timeout));
};
