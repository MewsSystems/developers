import { composableFetch, pipeP, tryCatchP } from 'composable-fetch';

const newError = (id, message, res) => {
  const error = new Error(message);
  error.id = id;
  if (res) error.res = res;
  return error;
};

const checkStatus = res => {
  if (res.status === 500)
    throw newError(
      'InvalidStatusCodeError',
      'Invalid status code: ' + res.status,
      res,
    );
  return res;
};

const handleFetchError = async error => {
  const { res } = error;
  const genericError = new Error('Fetch error');
  genericError.res = res;
  genericError.data = { message: 'An error occured.' };
  genericError.originalError = error;

  try {
    await composableFetch.decodeResponse(res);
    if (typeof res.data === 'object' && res.data.message)
      genericError.data = res.data;
  } catch (_) {}

  throw genericError;
};

export const fetchJSON = url =>
  tryCatchP(
    pipeP(
      composableFetch.withBaseUrl(url),
      composableFetch.withHeader('Accept', 'application/json'),
      composableFetch.withEncodedBody(JSON.stringify),
      composableFetch.retryable(
        pipeP(
          composableFetch.fetch1(fetch),
          checkStatus,
        ),
      ),
      composableFetch.withRetry(2),
      composableFetch.withSafe204(),
      composableFetch.decodeJSONResponse,
      composableFetch.checkStatus,
      ({ data }) => data,
    ),
    handleFetchError,
  );
