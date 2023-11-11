import { TMDB } from "tmdb-ts";

export const tmdbClient = new TMDB(process.env.TMDB_ACCESS_TOKEN as string);

export const MEDIA_300_BASE_URL = "https://image.tmdb.org/t/p/w300";
export const MEDIA_500_BASE_URL = "https://image.tmdb.org/t/p/w500";
export const MEDIA_ORIGINAL_BASE_URL = "https://image.tmdb.org/t/p/original";
