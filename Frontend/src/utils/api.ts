export const API_KEY = process.env.REACT_APP_API_KEY || '03b8572954325680265531140190fd2a'
export const API_ENDPOINT = process.env.REACT_APP_API_ENDPOINT || 'https://api.themoviedb.org/3'
export const API_ENDPOINT_IMAGE = 'https://image.tmdb.org/t/p'

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export async function callApi(method: string, url: string, data?: any) {
  const res = await fetch(url, {
    method,
    headers: {
      Accept: 'application/json'
    },
    body: JSON.stringify(data)
  })
  return res.json()
}
