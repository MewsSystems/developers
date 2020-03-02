import axios from 'axios';

import appConfig from 'config';

const axiosInstance = axios.create({
  baseURL: appConfig.BASE_URL,
});
// Axios doesn't support default params per instance:
// see https://github.com/axios/axios/issues/2190
axiosInstance.interceptors.request.use(config => ({
  ...config,
  params: {
    api_key: appConfig.API_KEY,
    language: appConfig.LANGUAGE,
    ...config.params,
  },
}));

export default axiosInstance;
