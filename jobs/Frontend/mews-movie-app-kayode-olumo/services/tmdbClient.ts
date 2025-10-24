import { TMDB_BASE_URL } from "@/constants";

export async function tmdbGet(path: string, init?: RequestInit) {
  const token = process.env.TMDB_ACCESS_TOKEN;
  if (!token) {
    throw new Error("TMDB_ACCESS_TOKEN is not set");
  }

  const options = {
    method: 'GET',
    headers: {
      accept: 'application/json',
      Authorization: `Bearer ${token}`
    },
    ...init,
  };

  const response = await fetch(`${TMDB_BASE_URL}${path}`, options);
  
  if (!response.ok) {
    throw new Error(`TMDB GET ${path} failed: ${response.status} ${response.statusText}`);
  }
  
  return response.json();
}