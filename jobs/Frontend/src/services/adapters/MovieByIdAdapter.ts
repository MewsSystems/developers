import { Movie, MovieSearchResult } from "@/types";
import { MovieDetailResponse, MovieServiceRespnse } from "../types";

const IMAGE_SOURCE = 'https://image.tmdb.org/t/p/w92';

export const MovieByIdAdapter = (response: MovieDetailResponse): Movie =>
    ({ id: response.id, title: response.title, overview: response.overview, posterImage: `${IMAGE_SOURCE}${response.poster_path}` });
