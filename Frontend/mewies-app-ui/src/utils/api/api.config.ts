import axios from 'axios'

export const dev = process.env.NODE_ENV !== 'production'

const baseURL = 'https://api.themoviedb.org/3'

const axiosInstance = axios.create({
    baseURL,
})

axiosInstance.interceptors.request.use(config => {
    config.params = config.params || {}
    config.params['api_key'] = '03b8572954325680265531140190fd2a'
    return config
})

export default axiosInstance
