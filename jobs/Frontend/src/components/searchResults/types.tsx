import { MovieSummary, MovieSearchResult } from "@/types";

export type ResultsListItemProps = { item: MovieSummary };
export type ResultsListProps = {
  results: MovieSearchResult | void;
};
