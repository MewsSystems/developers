import { FC, useEffect, useState } from "react";
import { HorizontalCentered } from "src/components/HorizontalCentered/HorizontalCentered";
import { useLazyGetMoviesQuery } from "src/store/slices/moviesSlice";
import { MovieType } from "src/store/types/MovieType";
import { PaginationQueryData } from "src/store/types/PaginationQueryData";
import { ReduxLazyHookReturn } from "src/store/types/ReduxLazyHookReturn";
import { InputSearch } from "src/components/InputSearch/InputSearch";
import { MovieList } from "src/views/MovieSearch/components/MovieList/MovieList";
import { PaginationControls } from "src/views/MovieSearch/components/PaginationControls/PaginationControls";

export const MovieSearch: FC = () => {
  const [inputValue, setInputValue] = useState("");
  const [page, setPage] = useState(1);

  const [fetchMovies, { isLoading, data }]: ReduxLazyHookReturn<
    PaginationQueryData<MovieType[]>
  > = useLazyGetMoviesQuery();

  useEffect(() => {
    if (!inputValue || inputValue === "") return;

    fetchMovies({ name: inputValue, page });
  }, [inputValue, page]);

  return (
    <HorizontalCentered>
      <h2>Movie Searcher</h2>
      <InputSearch
        debounceTime={500}
        onDebounce={(value: string) => {
          setInputValue(value);
          setPage(1);
        }}
        onEnter={(value: string) => {
          setInputValue(value);
          setPage(1);
        }}
        placeholder={"Search movies.."}
        loading={isLoading}
      />
      <MovieList movies={data?.results} />
      {data?.results.length ? (
        <PaginationControls
          page={Number(page)}
          totalPages={Number(data?.total_pages) || 0}
          setPage={setPage}
        />
      ) : null}
    </HorizontalCentered>
  );
};
