import { Input } from "@/components/ui/Input";
import { useMovieResultsList } from "@/hooks/queries/useMovieResultsList";
import { useSearchParams } from "react-router";
import { useDebounce } from "@/hooks/useDebounce";
import MovieCard from "@/pages/MoviePage/components/MovieCard";
import PaginationWrapper from "@/components/PaginationWrapper";
import { Skeleton } from "@/components/ui/Skeleton";
import ErrorAlert from "@/components/ErrorAlert";
import { AlertDescription } from "@/components/ui/Alert";

const MovieSearchPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const search = searchParams.get('search') ?? '';
  const page = Number(searchParams.get('page') ?? '1');
  const debouncedQuery = useDebounce(search, 500);
  const { data, isLoading, isError, error } = useMovieResultsList(debouncedQuery, page);

  const handlePageChange = (page: number) => {
    setSearchParams({ search, page: page.toString() });
  };

  const handleQueryChange = (query: string) => {
    setSearchParams({ page: page.toString(), search: query });
  };

  return (
    <div className="flex flex-col items-center gap-4 w-full">
      <Input
        value={search}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleQueryChange(e.target.value)}
      />

      {isLoading && (
        <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-4">
          {[...Array(4)].map((_, i) => (
            <Skeleton key={i} className="h-80 w-80" />
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
          <AlertDescription>
            No movies found. Try a different search.
          </AlertDescription>
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
