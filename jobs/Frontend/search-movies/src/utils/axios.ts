import Axios from "axios";
import { BASE_API_URL } from "./api";

export const axiosBrowse = Axios.create({
  baseURL: BASE_API_URL,
});

axiosBrowse.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    const message = error.response?.data?.message || error.message;
    console.error(message);

    return Promise.reject(error);
  }
);
