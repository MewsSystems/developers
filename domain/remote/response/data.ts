import { AxiosResponse } from "axios";

export const getAxiosData = <T>(response: AxiosResponse<T>) => response.data;
