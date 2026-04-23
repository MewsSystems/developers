import { MovieSearchResult } from "@/types";
import { MovieServiceRespnse } from "../types";
import { MovieByIdAdapter } from "./MovieByIdAdapter";
import { MovieSummaryAdapter } from "./MovieSummaryAdapter";

export const MovieSearchResponseAdapter = (response: MovieServiceRespnse): MovieSearchResult => ({
  page: response.page,
  totalPage: response.total_pages,
  movies: response.results.map((res) => MovieSummaryAdapter(res))
});