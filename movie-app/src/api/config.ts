import axios, { AxiosRequestConfig } from "axios";

const api = axios.create({
  headers: { "Content-Type": "application/json" },
  baseURL: "https://api.themoviedb.org/3/",
});

export const fetchAPI = async <TResponse>(
  path: string,
  options: AxiosRequestConfig = {},
  fallback: TResponse | null = null,
): Promise<TResponse | null> => {
  try {
    const { data } = await api<TResponse>(path, options);
    return data;
  } catch {
    return fallback;
  }
};

export const movieDbApiKey = import.meta.env.VITE_MOVIEDB_API_KEY;
