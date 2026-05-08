import ky from 'ky'

export const api = ky.create({
  prefixUrl: 'https://api.themoviedb.org/3/',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
    Authorization: `Bearer ${import.meta.env.VITE_BEARER_TOKEN}`,
  },
})
