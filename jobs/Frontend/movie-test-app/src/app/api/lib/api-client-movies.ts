import Axios, { InternalAxiosRequestConfig } from 'axios';

const baseUrl = import.meta.env.VITE_MOVIE_DB_BASE_URL;
const apiVersion = import.meta.env.VITE_MOVIE_DB_API_VERSION;
const apiKey = import.meta.env.VITE_MOVIE_DB_API_KEY;

function authRequestInterceptor(config: InternalAxiosRequestConfig) {
  config.params = { ...config.params, api_key: apiKey };

  return config;
}

const moviesApiClient = Axios.create({
  baseURL: `${baseUrl}/${apiVersion}`,
});

moviesApiClient.interceptors.request.use(authRequestInterceptor);

export { moviesApiClient };
