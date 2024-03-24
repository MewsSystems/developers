import axios from 'axios'

const api = axios.create({
  baseURL: process.env.PUBLIC_API_URL,
})

api.interceptors.request.use((config) => {
  config.params = config.params || {}
  config.params['api_key'] = process.env.PUBLIC_API_KEY
  return config
})

export default api
