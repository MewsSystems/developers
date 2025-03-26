import { Input } from "@/components/ui/Input";
import { useMovies } from "@/hooks/useMovieSearch";
import { useSearchParams } from "react-router";
import { useDebounce } from "@/hooks/useDebounce";
import MovieCard from "@/components/MovieCard";
import PaginationWrapper from "@/components/PaginationWrapper";
import { Skeleton } from "@/components/ui/Skeleton";
import ErrorAlert from "@/components/ErrorAlert";

const MovieSearchPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const search = searchParams.get('search') ?? '';
  const page = Number(searchParams.get('page') ?? '1');
  const debouncedQuery = useDebounce(search, 500);
  const { data, isLoading, isError, error } = useMovies(debouncedQuery, page);

  const handlePageChange = (page: number) => {
    setSearchParams({ search, page: page.toString() });
  };

  const handleQueryChange = (query: string) => {
    setSearchParams({ page: page.toString(), search: query });
  };

  return (
    <div className="flex flex-col gap-4">
      <Input
        value={search}
        onChange={(e) => handleQueryChange(e.target.value)}
      />

      {isLoading && (
        <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-4">
          {[...Array(4)].map((_, i) => (
            <Skeleton key={i} className="h-80 w-full" />
          ))}
        </div>
      )}

      {isError && <ErrorAlert error={error} />}

      {data && (
        data.totalResults > 0 ? (
          <div className="max-h-[70vh] overflow-y-auto md:grid md:grid-cols-2 lg:grid-cols-4 gap-4">
            {data.results.map((movie) => (
              <MovieCard key={movie.id} movie={movie} />
            ))}
          </div>
        ) : (
          <p>No movies found. Try a different search.</p>
        )
      )}
      {search.length > 0 && data && data.totalResults > 0 && (
        <PaginationWrapper
          currentPage={page}
          handlePageChange={handlePageChange}
          totalPages={data.totalPages} />
      )}
    </div>
  );
};

export default MovieSearchPage;
