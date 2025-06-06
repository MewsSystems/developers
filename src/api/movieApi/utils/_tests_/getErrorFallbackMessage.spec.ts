import {API_STATUS_MESSAGE, ERRORS_BY_HTTP_STATUS} from '../../constants';
import {getErrorFallbackMessage} from '../getErrorFallbackMessage';

describe('getErrorFallbackMessage', () => {
  describe('known http statuses', () => {
    describe('401', () => {
      it('should handle invalid API key error', () => {
        expect(
          getErrorFallbackMessage({
            status: 401,
            message: API_STATUS_MESSAGE.INVALID_API_KEY,
          }),
        ).toBe(ERRORS_BY_HTTP_STATUS[401][API_STATUS_MESSAGE.INVALID_API_KEY]);
      });

      it('should handle authentication failed error', () => {
        expect(
          getErrorFallbackMessage({
            status: 401,
            message: API_STATUS_MESSAGE.AUTHENTICATION_FAILED,
          }),
        ).toBe(ERRORS_BY_HTTP_STATUS[401][API_STATUS_MESSAGE.AUTHENTICATION_FAILED]);
      });

      it('should handle invalid token error', () => {
        expect(
          getErrorFallbackMessage({
            status: 401,
            message: API_STATUS_MESSAGE.INVALID_TOKEN,
          }),
        ).toBe(ERRORS_BY_HTTP_STATUS[401][API_STATUS_MESSAGE.INVALID_TOKEN]);
      });
    });

    describe('404', () => {
      it('should handle resource not found error', () => {
        expect(
          getErrorFallbackMessage({
            status: 404,
            message: API_STATUS_MESSAGE.RESOURCE_NOT_FOUND,
          }),
        ).toBe(ERRORS_BY_HTTP_STATUS[404][API_STATUS_MESSAGE.RESOURCE_NOT_FOUND]);
      });
    });

    describe('500', () => {
      it('should handle invalid ID error', () => {
        expect(
          getErrorFallbackMessage({
            status: 500,
            message: API_STATUS_MESSAGE.INVALID_ID,
          }),
        ).toBe(ERRORS_BY_HTTP_STATUS[500][API_STATUS_MESSAGE.INVALID_ID]);
      });

      it('should handle internal error', () => {
        expect(
          getErrorFallbackMessage({
            status: 500,
            message: API_STATUS_MESSAGE.INTERNAL_ERROR,
          }),
        ).toBe(ERRORS_BY_HTTP_STATUS[500][API_STATUS_MESSAGE.INTERNAL_ERROR]);
      });

      it('should handle generic failed error', () => {
        expect(
          getErrorFallbackMessage({
            status: 500,
            message: API_STATUS_MESSAGE.FAILED,
          }),
        ).toBe(ERRORS_BY_HTTP_STATUS[500][API_STATUS_MESSAGE.FAILED]);
      });
    });
  });

  describe('unknown errors', () => {
    it('should return default message for unknown status', () => {
      expect(
        getErrorFallbackMessage({
          status: 418,
          message: 'Random error message',
        }),
      ).toBe('An unexpected error occurred. Please try again later.');
    });

    it('should return default message for known status but unknown message', () => {
      expect(
        getErrorFallbackMessage({
          status: 401,
          message: 'Unexpected error message',
        }),
      ).toBe('An unexpected error occurred. Please try again later.');
    });
  });
});
