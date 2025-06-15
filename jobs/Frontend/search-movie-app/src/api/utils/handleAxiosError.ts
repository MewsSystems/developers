import axios, { AxiosError } from 'axios';
import type { ErrorApiResponse } from '../types';

const handleAxiosError = (error: unknown) => {
  if (axios.isAxiosError(error) && error.response) {
    const axiosError = error as AxiosError;
    const errorApiResponse = axiosError.response?.data as ErrorApiResponse;
    throw {
      status: errorApiResponse?.status_code,
      message:
        errorApiResponse?.status_message ||
        axiosError.message ||
        'An error occurred, please try again',
    };
  } else {
    const errorRequest = error as Error;
    throw {
      message: errorRequest.message,
    };
  }
};

export { handleAxiosError };
