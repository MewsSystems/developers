"use server";

import { TMDB_BASE_URL } from "@/constants";

function authHeaders() {
  const token = process.env.TMDB_ACCESS_TOKEN;
  if (!token) throw new Error("TMDB_ACCESS_TOKEN is not set");
  return {
    Authorization: `Bearer ${token}`,
    accept: "application/json",
  };
}

export async function tmdbGet(path: string, init?: RequestInit) {
  const url = `${TMDB_BASE_URL}${path}`;
  const response = await fetch(url, {
    headers: authHeaders(),
    cache: "no-store",
    ...init,
  });
  if (!response.ok) {
    throw new Error(`TMDB GET ${path} failed: ${response.status} ${response.statusText}`);
  }
  return response.json();
}