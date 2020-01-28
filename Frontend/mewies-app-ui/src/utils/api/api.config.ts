import axios from 'axios'

export const dev = process.env.NODE_ENV !== 'production'

const baseURL = process.env.API_URL

const axiosInstance = axios.create({
    baseURL,
})

axiosInstance.interceptors.request.use(config => {
    config.params = config.params || {}
    config.params['api_key'] = process.env.API_KEY
    return config
})

export default axiosInstance
