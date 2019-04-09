import config from '../../config'

export const getConfig = async () => {
  const result = await fetch(`${config.endpoint}/configuration`, {
    method: 'GET',
  }).then(response => {
    return !response.ok ? Promise.reject(response.statusText) : response.json()
  })

  return await result
}
