import React, { ChangeEvent, useEffect, useMemo } from "react";
import { MovieTile } from "../components/movie-tile";
import { debounce } from "lodash";
import styled from "styled-components";
import { InputText } from "../components/input";
import { useDispatch, useSelector } from "react-redux";
import { appActionCreators } from "../redux/actions";
import { AppReduxState } from "../redux/state";
import { MoviesPage } from "../types";
import { Pagination } from "../components/pagination";
import { appSelectors } from "../redux/selectors";
import { movieDbApi } from "../api/movie-db-api";

interface ISearchViewProps {
  children?: never;
}

export const SearchView: React.FC<ISearchViewProps> = () => {
  const dispatch = useDispatch();

  const searchValue = useSelector<AppReduxState, string>(
    appSelectors.getSearchMovieTitle
  );
  const currentPage = useSelector(appSelectors.getCurrentPage);
  const shouldFetchMovies = useSelector(appSelectors.shouldFetchMovies);

  const { movies, totalPages } = useSelector<AppReduxState, MoviesPage>(
    (state) => {
      const page = state.moviePages.find(
        (page) =>
          page.searchValue === searchValue && page.pageNumber === currentPage
      );
      return (
        page || { totalPages: 0, movies: [], pageNumber: 1, searchValue: "" }
      );
    }
  );

  const handleMovieTitleChange = (event: ChangeEvent<HTMLInputElement>) => {
    dispatch(appActionCreators.setSearchMovieTitle(event.target.value));
  };

  const handleNextPageClick = () =>
    dispatch(
      appActionCreators.setCurrentPage(
        currentPage < totalPages ? currentPage + 1 : totalPages
      )
    );

  const handlePreviousPageClick = () =>
    dispatch(
      appActionCreators.setCurrentPage(currentPage > 1 ? currentPage - 1 : 1)
    );

  const handleExactPageClick = (page: number) => {
    dispatch(appActionCreators.setCurrentPage(page));
  };

  const debouncedFetchMoviesByTitle = useMemo(
    () =>
      debounce((movieTitle: string, page: number) => {
        dispatch(appActionCreators.setLoading(true));
        movieDbApi
          .getMovies(movieTitle, page)
          .then((response) =>
            dispatch(
              appActionCreators.setMoviePage({
                pageNumber: page,
                totalPages: response.data.total_pages,
                movies: response.data.results,
                searchValue: movieTitle,
              })
            )
          )
          .finally(() => dispatch(appActionCreators.setLoading(false)));
      }, 1500),
    [dispatch]
  );

  // Data fetching
  useEffect(() => {
    if (searchValue && shouldFetchMovies) {
      debouncedFetchMoviesByTitle(searchValue, currentPage);
    }
  }, [
    searchValue,
    debouncedFetchMoviesByTitle,
    currentPage,
    shouldFetchMovies,
  ]);
  // Scrolling effect
  useEffect(() => {
    window.scrollTo({ top: 0, left: 0, behavior: "smooth" });
  }, [movies]);

  return (
    <SearchViewLayout>
      <InputContainer>
        <InputText
          value={searchValue}
          placeholder={"Find your favourite movie"}
          onChange={handleMovieTitleChange}
        />
      </InputContainer>
      <MoviesContainer>
        {movies.map((movie) => {
          const {
            id,
            original_title,
            overview,
            vote_average,
            vote_count,
            poster_path,
          } = movie;
          return (
            <MovieTile
              key={id}
              movieId={id}
              originalTitle={original_title}
              overview={overview}
              voteAverage={vote_average}
              voteCount={vote_count}
              posterSrc={poster_path}
            />
          );
        })}
      </MoviesContainer>
      <PaginationContainer>
        <Pagination
          currentPage={currentPage}
          maxPages={totalPages}
          onNextPageClick={handleNextPageClick}
          onPreviousPageClick={handlePreviousPageClick}
          onExactPageClick={handleExactPageClick}
        />
      </PaginationContainer>
    </SearchViewLayout>
  );
};

const TOP_VIEW_OFFSET = "4rem";

const SearchViewLayout = styled.div`
  width: 100%;
  height: 100%;
  display: grid;
  gap: 1rem;
  grid-template-columns: 1fr 1fr;
  grid-template-rows: 2rem 1fr 4rem;
  grid-template-areas:
    "search search"
    "movies movies"
    "pagination pagination";
  padding: ${TOP_VIEW_OFFSET} 0 0 0;
  background: ${(props) => props.theme.color.grey};
`;

const MoviesContainer = styled.div`
  grid-area: movies;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  min-height: 100vw;
  margin-top: ${TOP_VIEW_OFFSET};
  & > div {
    margin: 1rem;
    max-width: 45%;
    min-width: 30rem;
  }
`;

const InputContainer = styled.div`
  grid-area: search;
  display: flex;
  width: 100%;
  justify-content: center;

  & > input {
    width: 30rem;
  }
`;

const PaginationContainer = styled.div`
  grid-area: pagination;
`;
