import { composableFetch, pipeP, tryCatchP } from 'composable-fetch';

export const handleFetchError = async error => {
  const { res } = error;
  const genericError = new Error('Fetch error');
  genericError.res = res;
  genericError.data = { success: false, message: 'An error occured.' };
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
      composableFetch.fetch1(fetch),
      composableFetch.withSafe204(),
      composableFetch.decodeJSONResponse,
      composableFetch.checkStatus,
      ({ data }) => data,
    ),
    handleFetchError,
  );
