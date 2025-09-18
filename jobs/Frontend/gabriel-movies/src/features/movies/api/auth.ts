const VITE_TMDB_ACCESS_TOKEN = import.meta.env.VITE_TMDB_ACCESS_TOKEN as string | undefined;

if (!VITE_TMDB_ACCESS_TOKEN) {
  console.error("Missing VITE_TMDB_ACCESS_TOKEN");
}

export const tmdbHeaders: HeadersInit = {
  Accept: "application/json",
  Authorization: `Bearer ${VITE_TMDB_ACCESS_TOKEN}`,
};
