import { AxiosRequestConfig } from 'axios'

export type RequestConfig = Pick<
  AxiosRequestConfig,
  Exclude<
    keyof AxiosRequestConfig,
    'method' | 'url' | 'data' | 'params' | 'responseType'
  >
>
