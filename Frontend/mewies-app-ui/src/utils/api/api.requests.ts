import axios from './api.config'
import { AxiosRequestConfig } from 'axios'

export const getRequest = <K>(
    url: string,
    config?: AxiosRequestConfig
): Promise<K> =>
    axios
        .get(url, config)
        .then(res => res.data)
        .catch((err: Error) => {
            throw new Error(err.message)
        })

export const postRequest = <K, T>(
    url: string,
    body: K,
    config?: AxiosRequestConfig
): Promise<T> =>
    axios
        .post(url, body, config)
        .then(res => res.data)
        .catch((err: Error) => {
            throw new Error(err.message)
        })
