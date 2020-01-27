import axios from 'axios'

export const dev = process.env.NODE_ENV !== 'production'

const instance = axios

export default instance
