import api from './connectors';

/**
 * Create get request
 * @param {string} url
 * @param {Object} parameters
 * @return {Promise<>}
 */
export async function createGetRequest(url, parameters = {}) {
  return await api
    .parameters(parameters)
    .get(url)
    .send();
}