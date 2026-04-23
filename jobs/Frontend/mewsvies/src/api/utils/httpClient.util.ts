import axios from 'axios'
import { API_URL, API_KEY } from '../constants/config.constant.ts'

export const httpClient = axios.create({
    baseURL: `${API_URL}`,
    headers: {
        'Content-Type': 'application/json',
    },
    params: {
        api_key: `${API_KEY}`,
    },
})
