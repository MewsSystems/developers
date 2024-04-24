import { Movie, MovieSearchResult } from "@/types";

export type ResultsListItemProps = { item: Movie };
export type ResultsListProps = {
  results: MovieSearchResult | void;
};
