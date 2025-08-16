export const getUrlSearchParams = (params: any) => {
  const searchParams = new URLSearchParams();

  for (const [key, value] of Object.entries(params)) {
    if (value !== null && value !== undefined) {
      searchParams.append(key, value.toString());
    }
  }

  return searchParams;
};

export const fetchResults = async (
  queryParams: Record<string, string>,
  abortController: AbortController,
  baseApiUrl: string,
  headers: any,
): Promise<any> => {
  const queryParamsString = getUrlSearchParams(queryParams).toString();

  const requestUrl = queryParamsString ? `${baseApiUrl}?${queryParamsString}` : baseApiUrl;

  const response = await fetch(requestUrl, {
    method: 'GET',
    signal: abortController.signal,
    headers,
  });

  const data = await response.json();

  if (!response.ok) {
    throw {
      data,
      status: response.status,
      statusText: response.statusText,
    };
  }

  return {
    data,
    status: response.status,
    statusText: response.statusText,
  };
};
