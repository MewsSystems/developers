import axios from "axios";
import { apiConfig } from "./config";

export const api = axios.create({
  baseURL: apiConfig.baseUrl,
  headers: {
    "Content-Type": "application/json",
    Authorization: `Bearer ${apiConfig.accessToken}`,
  },
});
