import { Input } from "@/components/ui/Input";
import { useMovies } from "@/hooks/useMovieSearch";
import { useSearchParams } from "react-router";
import { useDebounce } from "@/hooks/useDebounce";
import MovieCard from "@/components/MovieCard";
import PaginationWrapper from "@/components/PaginationWrapper";

const MovieSearchPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const search = searchParams.get('search') ?? '';
  const page = Number(searchParams.get('page') ?? '1');
  const debouncedQuery = useDebounce(search, 500);
  const { data, isLoading } = useMovies(debouncedQuery, page);

  const handlePageChange = (page: number) => {
    setSearchParams({ search, page: page.toString() });
  };

  const handleQueryChange = (query: string) => {
    setSearchParams({ page: page.toString(), search: query });
  };

  return (
    <>
      <Input value={search} onChange={(e) => handleQueryChange(e.target.value)} />

      {isLoading && <p>Loading...</p>}

      {data && (
        <div className="max-h-80 overflow-y-auto">
          {data.results.map((movie) => (
            <MovieCard key={movie.id} movie={movie} />
          ))}
        </div>
      )}
      {search.length > 0 && data && (
        <PaginationWrapper currentPage={page} handlePageChange={handlePageChange} totalPages={data.totalPages} />
      )}
    </>
  );
};

export default MovieSearchPage;
