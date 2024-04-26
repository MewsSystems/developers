import axios from "axios";
import { API_KEY, ENDPOINT_URL_BASE } from "../../config/config";

const authenticatedApiClient = axios.create({
  baseURL: ENDPOINT_URL_BASE,
});

authenticatedApiClient.interceptors.request.use(
  (config) => {
    config.params = { ...config.params, api_key: API_KEY };
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

export default authenticatedApiClient;
