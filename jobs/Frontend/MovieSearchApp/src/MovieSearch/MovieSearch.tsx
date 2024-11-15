import { useState } from "react";
import { Input } from "../components/ui/input";
import { Pagination, PaginationProps } from "./Pagination";
import { useDebounce } from "@/components/hooks/useDebounce";
import { useGetMovies } from "./useGetMovies";
import { MovieRow, MovieList } from "./MovieList";
import { ErrorMessage } from "@/components/ui/error-message";
import { extractYearFromReleaseDate } from "../../utils";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { MovieDetail } from "@/MovieDetail/MovieDetail";
import { DialogTitle } from "@radix-ui/react-dialog";
import { VisuallyHidden } from "@radix-ui/react-visually-hidden";
import { keepPreviousData } from "@tanstack/react-query";

/**
 * Movie search component.
 * Renders a search input.
 * When search yields results, renders a table with results and a paginator.
 * When search yields no results, renders a no results message.
 * When search ends with an error, renders an error message.
 */
export const MovieSearch = () => {
  const [searchValue, setSearchValue] = useState<string>("");

  const debouncedSearchValue = useDebounce(searchValue);

  return (
    <section className="m-auto max-w-4xl h-full">
      <div className="absolute left-4 right-4 m-auto max-w-4xl">
        <Input
          aria-label="search movies"
          value={searchValue}
          onChange={(event) => setSearchValue(event.currentTarget.value)}
          placeholder="Search movies..."
          className="h-12"
        />
      </div>
      {debouncedSearchValue && <MovieSearchContent searchValue={debouncedSearchValue} />}
    </section>
  );
};

type MovieSearchContentProps = {
  searchValue: string;
};

const MovieSearchContent: React.FC<MovieSearchContentProps> = ({ searchValue }) => {
  const [page, setPage] = useState<number>(1);
  const [selectedMovie, setSelectedMovie] = useState<number | undefined>();

  const { data, isError, isPlaceholderData } = useGetMovies({
    movieTitle: searchValue,
    page,
    placeholderData: keepPreviousData,
  });

  if (isError) {
    return <ErrorMessage />;
  }

  if (!data) {
    return null;
  }

  const movies: Array<MovieRow> =
    data?.results.map((movie) => ({
      id: movie.id,
      originalLanguage: movie.original_language,
      originalTitle: movie.original_title,
      releaseDate: extractYearFromReleaseDate(movie.release_date),
    })) ?? [];

  if (!movies.length) {
    return <NoResultsMessage />;
  }

  const paginationProps: PaginationProps = {
    page: data?.page ?? 0,
    pageCount: data?.total_pages ?? 0,
    goToPage: setPage,
    isDataLoading: isPlaceholderData,
  };

  const onTableRowClick = (movieId: number) => {
    setSelectedMovie(movieId);
  };

  return (
    <div className="pt-16">
      <MovieList movies={movies} onTableRowClick={onTableRowClick} />
      <Pagination {...paginationProps} />
      {selectedMovie && (
        <Dialog open={!!selectedMovie} onOpenChange={() => setSelectedMovie(undefined)}>
          <DialogContent className="min-h-[425px] min-w-[400px] w-11/12 max-w-[1000px]">
            <VisuallyHidden>
              <DialogTitle />
            </VisuallyHidden>
            <MovieDetail movieId={selectedMovie} />
          </DialogContent>
        </Dialog>
      )}
    </div>
  );
};

const NoResultsMessage = () => {
  return <div className="h-full flex justify-center items-center">Found 0 movies.</div>;
};
