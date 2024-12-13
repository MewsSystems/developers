import { ChangeEvent, useCallback, useContext, useState } from "react";
import { DiscoverMovieResponse, Movie } from "@/types/movies";
import fetchBase from "@/utils/fetchBase";
import useDebounce from "@/hooks/useDebounce";
import SearchBar from "@/components/SearchBar/SearchBar";
import MovieCard from "@/components/MovieCard/MovieCard";
import { MainContent } from "@/pages/Main/MainStyle";
import useInfiniteScroll from "@/hooks/useInfiniteScroll";
import ErrorContext from "@/providers/ErrorContext";
import createQueryString from "@/utils/createQueryString";
import SkeletonsGrid from "@/components/SkeletonsGrid/SkeletonsGrid";
import NoResults from "@/components/NoResults/NoResults";

interface FetchMoviesArgs {
  page?: number;
  search?: string;
}

const Main: React.FC = () => {
  const [searchValue, setSearchValue] = useState<string>("");
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState<number | null>(null);
  const { setMessage } = useContext(ErrorContext);

  const fetchMovies = useCallback(
    async (queries: FetchMoviesArgs) => {
      const { page, search } = queries || {};
      const queryString = createQueryString({
        page: page || currentPage,
        query: search || searchValue,
      });

      setLoading(true);
      try {
        const response = await fetchBase(`/search/movie${queryString}`);
        const data = (await response.json()) as DiscoverMovieResponse;
        setMovies((movies) =>
          search ? data.results : [...movies, ...data.results]
        );
        setCurrentPage(data.page);
        setTotalPages(data.total_pages);
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
      } catch (_err) {
        setMessage("Error during fetching movies occured");
      } finally {
        setLoading(false);
      }
    },
    [currentPage, setMessage, searchValue]
  );

  useInfiniteScroll(() => {
    if (totalPages && totalPages > currentPage)
      return fetchMovies({ page: currentPage + 1 });
  });

  const debounceFetchMovies = useDebounce<[FetchMoviesArgs]>(fetchMovies, 1000);

  const handleSearch = ({
    target: { value },
  }: ChangeEvent<HTMLInputElement>) => {
    setSearchValue(value);
    if (value) debounceFetchMovies({ search: value, page: 1 });
  };

  return (
    <>
      <SearchBar
        value={searchValue}
        onChange={handleSearch}
        placeholder="Search for the movie..."
      />
      {!loading && !movies.length ? (
        <NoResults />
      ) : (
        <MainContent>
          {movies.map((movie) => (
            <MovieCard {...movie} key={movie.id} />
          ))}
          {loading && <SkeletonsGrid />}
        </MainContent>
      )}
    </>
  );
};

export default Main;
