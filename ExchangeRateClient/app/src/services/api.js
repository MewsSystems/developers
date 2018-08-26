import { endpoint } from '../../config'
export const api = {
  fetchConfig,
  fetchRates,
}

async function fetchConfig () {
  var response = await fetch(endpoint + 'configuration')
  if (response.ok) {
    var json = await response.json();
    return json;
  }
  var error = await response.json()
  throw new Error(error)
}

async function fetchRates (ids) {
  if(ids.length === 0) return {};
  let query = '?'
  ids.forEach(id => query += 'currencyPairIds=' + id + '&');
  if(ids.length === 1)query += 'currencyPairIds=dummy&'
  var response = await fetch(endpoint + 'rates' + query.slice(0, -1))
  if (response.ok) {
    var json = await response.json();
    return json;
  }
  var error = await response.json()
  throw new Error(error)
}