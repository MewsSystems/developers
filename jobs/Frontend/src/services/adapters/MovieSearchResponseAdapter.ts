import { MovieSearchResult } from "@/types";
import { MovieServiceRespnse } from "../types";
import { MovieByIdAdapter } from "./MovieByIdAdapter";

const IMAGE_SOURCE = 'https://image.tmdb.org/t/p/w92';

export const MovieSearchResponseAdapter = (response: MovieServiceRespnse): MovieSearchResult => ({
  page: response.page,
  totalPage: response.total_pages,
  movies: response.results.map((res) => MovieByIdAdapter(res))
});