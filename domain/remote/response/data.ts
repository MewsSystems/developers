import { AxiosResponse } from "axios";

export interface Data<T> {
  results: T[];
  page: number;
  total_pages: number;
  total_results: number;
}

export const getAxiosData = <T>(response: AxiosResponse<T>) => response.data;

export const getRemoteResults = <T>(response: AxiosResponse<Data<T>>) =>
  getAxiosData(response).results;
