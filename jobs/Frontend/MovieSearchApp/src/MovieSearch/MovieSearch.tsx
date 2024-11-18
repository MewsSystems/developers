import { useState } from "react";
import { Input } from "../components/ui/input";
import { Pagination, PaginationProps } from "./Pagination";
import { useDebounce } from "@/components/hooks/useDebounce";
import { useGetMovies } from "./useGetMovies";
import { MovieRow, MovieList } from "./MovieList";
import { extractYearFromReleaseDate } from "../../utils";
import { MovieDetail } from "@/MovieDetail/MovieDetail";
import { keepPreviousData } from "@tanstack/react-query";
import { Frown } from "lucide-react";

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
    <>
      <div className="pb-4">
        <Input
          aria-label="search movies"
          value={searchValue}
          onChange={(event) => setSearchValue(event.currentTarget.value)}
          placeholder="Search movies..."
          className="h-12"
          autoFocus
        />
      </div>
      {debouncedSearchValue ? (
        <MovieSearchContent searchValue={debouncedSearchValue} />
      ) : (
        <div className="center-items text-2xl">Search TheMovieDB for movies!</div>
      )}
    </>
  );
};

type MovieSearchContentProps = {
  searchValue: string;
};

const MovieSearchContent: React.FC<MovieSearchContentProps> = ({ searchValue }) => {
  const [page, setPage] = useState<number>(1);
  const [selectedMovie, setSelectedMovie] = useState<{ id: number; title: string } | undefined>();

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

  const onTableRowClick = (movieId: number, movieTitle: string) => {
    setSelectedMovie({ id: movieId, title: movieTitle });
  };

  return (
    <>
      <MovieList movies={movies} onTableRowClick={onTableRowClick} />
      <Pagination {...paginationProps} />
      {!!selectedMovie && (
        <MovieDetail
          movieId={selectedMovie.id}
          movieTitle={selectedMovie.title}
          onClose={() => setSelectedMovie(undefined)}
        />
      )}
    </>
  );
};

const NoResultsMessage = () => {
  return <div className="text-2xl center-items">Found 0 movies.</div>;
};

const ErrorMessage = () => {
  return (
    <div className="text-2xl center-items">
      <Frown className="mr-2" />
      Ooops, something went wrong.
    </div>
  );
};
