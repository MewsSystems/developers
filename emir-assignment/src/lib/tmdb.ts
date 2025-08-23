import type { HttpError } from "./errors";

const BASE_URL = "https://api.themoviedb.org/3";

const API_KEY = import.meta.env.VITE_TMDB_API_KEY as string | undefined;

if (!API_KEY) {
    console.error("[TMDB] Missing VITE_TMDB_API_KEY in .env");
}

function toSearchParams(params?: Record<string, unknown>) {
    const sp = new URLSearchParams();
    if (!params) return sp;

    Object.entries(params).forEach(([k, v]) => {
        if (v === undefined || v === null) return;
        sp.set(k, String(v));
    });
    return sp;
}

type FetchJsonInit = Omit<RequestInit, "headers"> & {
    signal?: AbortSignal;
};

/**
 * Minimal fetch wrapper for TMDB v3.
 * - Always appends api_key param
 * - Adds default params: language=en-US, include_adult=false
 * - Throws on non-2xx with useful error info
 */
export async function fetchJson<T = unknown>(
    path: string,
    params?: Record<string, unknown>,
    init?: FetchJsonInit
): Promise<T> {
    const base = BASE_URL.replace(/\/$/, "");
    const cleanedPath = path.replace(/^\//, "");
    const url = new URL(`${base}/${cleanedPath}`);

    const search = toSearchParams({
        api_key: API_KEY,
        language: "en-US",
        include_adult: false,
        ...params,
    });

    url.search = search.toString();

    const res = await fetch(url.toString(), init);

    if (!res.ok) {
        let body: unknown = null;
        try {
            body = await res.json();
        } catch {
            /* ignore */
        }
        const error: HttpError = new Error(
            `[TMDB] ${res.status} ${res.statusText}${
                (body as { status_message?: string })?.status_message
                    ? ` — ${
                          (body as { status_message?: string }).status_message
                      }`
                    : ""
            }`
        );
        error.status = res.status;
        error.data = body;
        error.url = url.toString();
        throw error;
    }

    return (await res.json()) as T;
}

/** Image CDN base for TMDB */
const IMG_BASE = "https://image.tmdb.org/t/p/";

export type ImgSize =
    | "w92"
    | "w154"
    | "w185"
    | "w342"
    | "w500"
    | "w780"
    | "w1280"
    | "original";

/**
 * Build TMDB image URL for a given file path and size.
 * Returns empty string for falsy paths (so components can handle fallback).
 */
export function imgUrl(
    filePath?: string | null,
    size: ImgSize = "w500"
): string {
    if (!filePath) return "";
    return `${IMG_BASE}${size}${filePath}`;
}

export const posterUrl = (p?: string | null, size: ImgSize = "w342") =>
    imgUrl(p, size);
export const backdropUrl = (p?: string | null, size: ImgSize = "w1280") =>
    imgUrl(p, size);

// Common types
export type TmdbPage<T> = {
    page: number;
    results: T[];
    total_pages: number;
    total_results: number;
};

export type TmdbMovie = {
    id: number;
    title: string;
    overview: string;
    poster_path: string | null;
    backdrop_path: string | null;
    release_date?: string;
    vote_average: number;
    vote_count: number;
    genre_ids?: number[];
};

export type TmdbVideo = {
    id: string;
    key: string;
    site: string;
    type: string;
    name: string;
};

export type TmdbImages = {
    backdrops: { file_path: string; width: number; height: number }[];
    posters: { file_path: string; width: number; height: number }[];
};
