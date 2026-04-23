import { MovieSummary, MovieSearchResult } from "@/types";
import { MovieSummaryResponse } from "../types";
import { IMAGE_THUMBNAIL_SOURCE } from "@/constants";

export const MovieSummaryAdapter = (response: MovieSummaryResponse): MovieSummary =>
    ({ id: response.id, title: response.title, overview: response.overview, posterImage: `${IMAGE_THUMBNAIL_SOURCE}${response.poster_path}` });
