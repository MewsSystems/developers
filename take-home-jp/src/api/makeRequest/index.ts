import { BASE_API_URL } from '../endpoints';

const FETCH_OPTIONS = {
  method: 'GET',
  headers: {
    accept: 'application/json',
  },
};

export const makeRequest = (
  endpoint: string,
  urlParams: { [key: string]: unknown } = {},
) => {
  return fetch(
    `${BASE_API_URL}${endpoint}${new URLSearchParams({
      ...urlParams,
      api_key: `${process.env.REACT_APP_API_KEY}`,
    })}`,
    FETCH_OPTIONS,
  );
};
