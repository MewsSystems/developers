import { useSelector } from "react-redux";

import { RootState, useAppDispatch } from "../../store";
import { MoviesList, searchMovie } from "../../containers/Movies";
import { SearchInput } from "../../components/SearchInput";
import { LoadMoreSentinel } from "../../components/LoadMoreSentinel";
import { Helmet } from "react-helmet";

const selector = (s: RootState) => ({
  fetchState: s.fetchState,
  currentPage: s.currentPage,
  searchQuery: s.searchQuery,
  totalPages: s.totalPages,
});

export const Search = () => {
  const { fetchState, currentPage, searchQuery, totalPages } =
    useSelector(selector);

  const dispatch = useAppDispatch();

  const handleChangeSearch = (query: string) => dispatch(searchMovie(query));
  return (
    <>
      <Helmet>
        <title>Find your favorite movie!</title>
      </Helmet>
      <header>
        <h1 style={{ textAlign: "center" }}>Find your favorite movie!</h1>
      </header>
      <main>
        <SearchInput onChange={handleChangeSearch} defaultQuery={searchQuery} />

        <MoviesList />

        {fetchState === "success" && totalPages > currentPage && (
          <LoadMoreSentinel
            onLoadMore={() =>
              dispatch(searchMovie(searchQuery, currentPage + 1))
            }
          />
        )}

        {fetchState === "error" && <p>Something went wrong.</p>}
      </main>
    </>
  );
};
