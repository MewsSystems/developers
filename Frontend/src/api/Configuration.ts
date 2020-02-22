import axios, { AxiosPromise } from 'axios'
import { RequestConfig } from './requestConfig'
import { Configuration } from 'model/api/Configuration'

export class ConfigurationApi {
  /**
   * Configuration
   *
   * @param config The AxiosRequestConfig properties not predefined by the generated API
   */

  static getConfiguration(config?: RequestConfig): AxiosPromise<Configuration> {
    return axios({
      ...config,
      method: 'GET',
      url: `/configuration`,
    })
  }
}
