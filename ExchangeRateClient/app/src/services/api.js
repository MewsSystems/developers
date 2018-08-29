import { endpoint } from '../../config'
export const api = {
  fetchConfig,
  fetchRates,
}
import { _reduce, _mapValues } from '../utils/lodash'

const normalizer = (val) => ({
  selected: false,
  baseCode: val[0].code,
  baseName: val[0].name,
  secondaryCode: val[1].code,
  secondaryName: val[1].name,
  oldRate: null,
  newRate: null,
})

async function fetchConfig () {
  var response = await fetch(endpoint + 'configuration')
  if (response.ok) {
    var json = await response.json()
    debugger
    return _mapValues(normalizer, json.currencyPairs)
  }
  var error = await response.json()
  throw new Error(error)
}

async function fetchRates (pairs) {
  const query = _reduce((res, {selected}, id) => {
    if(selected) res += "&currencyPairIds=" + id
    return res
  }, "?currencyPairIds=dummy", pairs) //In case of 1 id ==> array

  var response = await fetch(endpoint + 'rates' + query)
  if (response.ok) {
    var json = await response.json()
    return json
  }
  var error = await response.json()
  throw new Error(error)
}