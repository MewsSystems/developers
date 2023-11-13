import axios from "axios";
import queryString from "query-string";
import { apiConfig } from "./config";

export const api = axios.create({
  baseURL: apiConfig.baseUrl,
  headers: { "Content-Type": "application/json" },
  paramsSerializer: (params: any) =>
    queryString.stringify({ ...params, api_key: apiConfig.apiKey }),
});
