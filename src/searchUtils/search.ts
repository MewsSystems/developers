import { SearchResponse } from '../types';
import { fetchResults } from './utils';

export const search = async (
  params: Record<string, string>,
  onDataRecieved: (data: SearchResponse) => void,
  abortController: AbortController,
  baseApiUrl: string,
) => {
  const headers = new Headers();
  headers.append('Accept', 'application/json');

  let response;

  try {
    response = await fetchResults(params, abortController, baseApiUrl, headers);

    onDataRecieved(response.data);
  } catch (error: any) {
    if (abortController.signal.aborted) return;
    return;
  }
};
