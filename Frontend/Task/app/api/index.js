import api from './connectors';

export async function createGetRequest(url, parameters = {}) {
  return await api
    .parameters(parameters)
    .get(url)
    .send();
}