import axios, { AxiosError, AxiosRequestConfig } from "axios";

const BASE_URL = 'https://api.themoviedb.org/3/';
const API_TOKEN = import.meta.env.VITE_API_TOKEN as string;

const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
    Authorization: `Bearer ${API_TOKEN}`,
  },
  timeout: 10000,
});

export type ErrorReason =
  | 'network'
  | 'not found'
  | 'unauthorized'
  | 'forbidden'
  | 'unreachable'
  | 'unknown'
  | 'invalid-response'
  | 'timeout';

export const throwError = (reason: ErrorReason): never => {
  throw Error(reason);
};

export const apiFetch = async <T>(url: string, options: AxiosRequestConfig = {}): Promise<T> => {
  try {
    const response = await api.get<T>(url, options);
    return response.data;
  } catch (error) {
    const axiosError = error as AxiosError;

    if (axiosError?.code === "ERR_NETWORK") {
      throwError('network');
    }
    if (axiosError?.code === "ECONNABORTED") {
      throwError('timeout');
    }

    switch (axiosError?.status) {
      case 404: {
        return throwError('not found');
      }
      case 401: {
        return throwError('unauthorized');
      }
      case 403: {
        return throwError('forbidden');
      }
      case 500:
      case 502: {
        return throwError('unreachable');
      }
      default: {
        return throwError('unknown');
      }
    }
  }
};
