import {ERRORS_BY_HTTP_STATUS} from '../constants';
import type {ApiErrorResponseDetails, ErrorMessages, ErrorStatus} from '../types';

export const getErrorFallbackMessage = ({status, message}: ApiErrorResponseDetails): string => {
  if (status in ERRORS_BY_HTTP_STATUS) {
    const httpStatus = status as ErrorStatus;
    const errorMessages = ERRORS_BY_HTTP_STATUS[httpStatus];

    if (message in errorMessages) {
      return errorMessages[message as ErrorMessages<typeof httpStatus>];
    }
  }

  return 'An unexpected error occurred. Please try again later.';
};
