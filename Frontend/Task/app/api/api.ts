import * as config from "../config.json";
const url = require("url");

// @ts-ignore
export async function fetchApi<T>(endpoint: string): Promise<T> {
  try {
    const response = await fetch(`${config.url}/${endpoint}`);
    return response.json();
  } catch (e) {
    throw new Error(e);
  }
}

export function convertToParams(params) {
  return url.format({ query: params });
}
