import type { HttpError } from "../../lib/errors";

export function prettySearchError(err: Error | null): {
    title: string;
    message?: string;
} {
    if (!err) return { title: "Search failed" };
    const http = err as HttpError;
    const s = http.status;

    if (s === 401 || s === 403) {
        return {
            title: "TMDB authentication failed",
            message:
                "Check VITE_TMDB_API_KEY and any API restrictions on your TMDB account.",
        };
    }
    if (s === 429) {
        return {
            title: "Rate limited by TMDB",
            message: "Too many requests. Please try again in a moment.",
        };
    }
    if (s && s >= 500) {
        return {
            title: "TMDB is having issues",
            message: `Server error (${s}). Please retry.`,
        };
    }
    return { title: "Search failed", message: err.message };
}
