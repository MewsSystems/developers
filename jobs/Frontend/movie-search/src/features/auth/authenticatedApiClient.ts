import axios from 'axios';
import { API_KEY, ENDPOINT_URL_BASE } from '../../configs/config';

/**
 * Creates a configured Axios instance for authenticated API requests.
 *
 * - Base URL is set to the provided ENDPOINT_URL_BASE.
 * - A request interceptor automatically adds the API key to all requests.
 */
const authenticatedApiClient = axios.create({
  baseURL: ENDPOINT_URL_BASE
});

authenticatedApiClient.interceptors.request.use(
  /**
   * Request interceptor to add the API key as a parameter.
   *
   * @param config - The configuration object for the outgoing request.
   * @returns The modified configuration object with the added API key parameter.
   */
  config => {
    config.params = { ...config.params, api_key: API_KEY, language: 'en-US' };
    return config;
  },
  /**
   * Error handler for request configuration errors.
   *
   * @param error - The error object if an error occurs during configuration.
   * @returns A rejected Promise with the original error.
   */
  error => {
    return Promise.reject(error);
  }
);

export default authenticatedApiClient;
