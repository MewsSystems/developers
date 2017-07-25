import request from 'client/api/request'
import {endpoint} from 'config/config'

export function getConfiguration () {
  return request.get(`${endpoint}/configuration`)
    .then(
      (response) => response.json(),
    )
}

export function getRates (currencyPairIds = {}) {
  // Map the currencyPairIds to an URL-friendly string
  const params = currencyPairIds.map((pair, index) => (
    `${index === 0 ? '?' : '&'}currencyPairIds[]=${encodeURI(pair)}`
  )).join('')

  return request.get(`${endpoint}/rates${params}`)
    .then(
      (response) => response.json(),
    )
}
