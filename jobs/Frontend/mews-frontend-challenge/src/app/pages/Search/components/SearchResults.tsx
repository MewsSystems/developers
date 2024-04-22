import { Separator } from "@/design-system/components/ui/separator";
import { BaseComponentProps } from "@/types";

import { MovieSearchResponse } from "../Search.data";
import { Movie } from "./Movie";
import { isEmpty } from "@/design-system/lib/utils";

export type SearchResultsProps = {
  searchQuery?: string;
  data: MovieSearchResponse;
};

export function SearchResults({
  data: { movies, pagination },
  searchQuery,
}: SearchResultsProps) {
  if (!searchQuery) {
    return null;
  }

  if (isEmpty(movies)) {
    return <EmptyResults />;
  }

  return (
    <>
      <h1 className="text-sm font-bold uppercase">
        Found {pagination.totalResults} matches for {searchQuery}
      </h1>
      <Separator className="mb-4 mt-1" />
      <ul className="flex flex-col gap-4">
        {movies.map((movie) => (
          <li key={movie.id}>
            <Movie movie={movie} />
          </li>
        ))}
      </ul>
    </>
  );
}

function EmptyResults(props: BaseComponentProps) {
  return (
    <div {...props}>
      <h1 className="text-sm font-bold uppercase">No results</h1>
      <Separator className="mb-4 mt-1" />
      <p>There were no matches for your search term.</p>
    </div>
  );
}
