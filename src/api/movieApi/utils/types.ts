import type {ERRORS_BY_HTTP_STATUS} from '../constants';

export type ErrorStatus = keyof typeof ERRORS_BY_HTTP_STATUS;
export type ErrorMessages<T extends ErrorStatus> = keyof (typeof ERRORS_BY_HTTP_STATUS)[T];

export type ApiErrorResponseDetails = {
  status: number;
  message: string;
};
