/**
 * Backend API path
 * @param path
 */
const apiPath = path => (
  `http://localhost:3000/${path}`
);

export const CONFIGURATION = apiPath('configuration');
export const RATES = apiPath('rates');
