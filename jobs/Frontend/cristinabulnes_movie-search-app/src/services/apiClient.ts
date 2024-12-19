import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";

const API_BASE_URL = process.env.VITE_API_BASE_URL;
const API_KEY = process.env.VITE_API_KEY;

const axiosParams: AxiosRequestConfig = {
	baseURL: API_BASE_URL,
	params: { api_key: API_KEY },
};

// Create an axios instance with specified parameters
const axiosInstance: AxiosInstance = axios.create(axiosParams);

type ApiMethods = {
	get: <T = any>(
		url: string,
		config?: AxiosRequestConfig
	) => Promise<AxiosResponse<T>>;
};

const api = (axios: AxiosInstance): ApiMethods => {
	return {
		get: (url, config = {}) => axios.get(url, config),
	};
};

export default api(axiosInstance);
