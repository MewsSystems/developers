import config from '../../config'

export const getRates = async params => {
  let query = ''
  params.forEach(item => {
    query += `currencyPairIds=${item}&`
  })
  query = query.slice(0, -1)

  const result = await fetch(`${config.endpoint}/rates?${query}`, {
    method: 'GET',
  }).then(response => {
    return !response.ok ? Promise.reject(response.statusText) : response.json()
  })

  return await result
}
