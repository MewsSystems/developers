"use server";

import { TMDB_BASE_URL } from "@/lib/constants";

function authHeaders() {
  const token = process.env.TMDB_ACCESS_TOKEN;
  if (!token) throw new Error("TMDB_ACCESS_TOKEN is not set");
  return { Authorization: `Bearer ${token}`, accept: "application/json" };
}

export async function tmdbGet(path: string, init?: RequestInit) {
  const res = await fetch(`${TMDB_BASE_URL}${path}`, { headers: authHeaders(), cache: "no-store", ...init });
  if (!res.ok) throw new Error(`TMDB GET ${path} failed: ${res.status} ${res.statusText}`);
  return res.json();
}
