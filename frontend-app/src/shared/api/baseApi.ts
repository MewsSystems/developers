export async function basePostApi({
  version,
  payload,
  path,
  params = {},
}: {
  version: string;
  payload: Record<string, string | number | boolean>;
  path: string;
  params?: Record<string, string>;
}) {
  const url = createUrl({
    version,
    path,
    params,
  });

  const response = await fetch(url, {
    method: "POST",
    headers: getBasicHeaders(),
    body: JSON.stringify(payload),
  });

  return (await response.json()) as any;
}

export async function baseGetApi<T>({
  path = "",
  params = {},
  version,
}: {
  path?: string;
  params?: Record<string, string>;
  version: string;
}): Promise<T | undefined> {
  const url = createUrl({ version, params, path });

  try {
    const response = await fetch(url, {
      method: "GET",
      headers: getBasicHeaders(),
    });
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }
    return await response.json();
  } catch (error: any) {
    console.error(error?.message);
    return undefined;
  }
}

function createUrl({
  version,
  path = "",
  params = {},
}: {
  version: string;
  path?: string;
  params?: Record<string, string>;
}) {
  const API_KEY = getApiKey();
  const searchParams = new URLSearchParams({ api_key: API_KEY });
  Object.entries(params).forEach(([key, value]) => {
    searchParams.append(key, value);
  });

  const url = new URL(
    `${version}/${path}`,
    "https://api.themoviedb.org/${version}"
  );

  return `${url}?${searchParams.toString()}`;
}

function getApiKey() {
  return import.meta.env["VITE_TMDB_API_KEY"];
}

function getBasicHeaders() {
  const myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");
  myHeaders.append("accept", "application/json");
  return myHeaders;
}
