import { TMDB } from "tmdb-ts";

export const tmdbClient = new TMDB(process.env.TMDB_ACCESS_TOKEN as string);

/**
 * This is a list of available sizes for media in v3 API.
 * https://developer.themoviedb.org/reference/configuration-details
 *
 * Available backdrop sizes: w300, w780, w1280, original
 * Available logo sizes: w45, w92, w154, w185, w300, w500, original
 * Available poster sizes: w92, w154, w185, w342, w500, w780, original
 * Available profile sizes: w45, w185, h632, original
 * Available still sizes: w92, w185, w300, original
 */
export const MEDIA_185_BASE_URL = "https://image.tmdb.org/t/p/w300";
export const MEDIA_300_BASE_URL = "https://image.tmdb.org/t/p/w300";
export const MEDIA_500_BASE_URL = "https://image.tmdb.org/t/p/w500";
export const MEDIA_ORIGINAL_BASE_URL = "https://image.tmdb.org/t/p/original";
