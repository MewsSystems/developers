import { useEffect } from "react";
import { useMovieContext } from "../../contexts/MovieContext";
import { MovieList } from "../MovieList/MovieList";
import CONSTANTS from "../../constants/Config";
import { Link, useParams } from "react-router-dom";
import { PageButton } from "../Pagination/PaginationStyles";
import LABEL from "../../constants/Labels";

import {
  BodyContainer,
  Heading,
  Subheading,
  SearchInput,
  MovieListWrapper,
  SkeletonGrid,
  SkeletonCard,
  Logo,
} from "./BodyStyles";

export const Body = () => {
  const { page } = useParams<{ page: string }>();
  const {
    searchMovies,
    loading,
    setLoading,
    searchMoviesByQuery,
    searchQuery,
    setSearchQuery,
    setCurrentPage,
    currentPage,
    error,
    debouncedQuery,
    setDebouncedQuery,
  } = useMovieContext();

  useEffect(() => {
    setLoading(true);
    const timeout = setTimeout(() => {
      setDebouncedQuery(searchQuery);
    }, 500);

    // Clear timeout if the component unmounts or searchQuery changes
    return () => clearTimeout(timeout);
  }, [searchQuery]);

  useEffect(() => {
    const newPage = page ? parseInt(page) : 1;

    //type check as page can be a string
    if (isNaN(newPage) || newPage < 1) {
      setCurrentPage(1);
      return;
    }

    setCurrentPage(newPage);
  }, [page, setCurrentPage]);

  useEffect(() => {
    // If there is a search query, fetch movies based on that query
    if (debouncedQuery && debouncedQuery.trim() !== "") {
      searchMoviesByQuery(debouncedQuery, currentPage);
    } else if (debouncedQuery === "" || debouncedQuery === undefined) {
      // If no search query, fetch popular movies default list
      searchMovies();
    }
  }, [currentPage, debouncedQuery]);

  if (error) {
    return (
      <BodyContainer>
        <Heading>{LABEL.BODY_HEADING}</Heading>
        <Logo src={CONSTANTS.LOGO} alt="Movie Search App Logo" />
        <br />
        <Link to="/movies/1" onClick={() => setSearchQuery("")}>
          <PageButton>Home</PageButton>
        </Link>
        <Subheading>{LABEL.ERROR}</Subheading>
        <Subheading>{error}</Subheading>
      </BodyContainer>
    );
  }

  return (
    <BodyContainer>
      <Heading> {LABEL.BODY_HEADING}</Heading>
      <Logo src={CONSTANTS.LOGO} alt="Movie Search App Logo" />
      <br />
      <Link to="/movies/1" onClick={() => setSearchQuery("")}>
        <PageButton>Home</PageButton>
      </Link>
      <Subheading>Search for your favorite movies</Subheading>
      <Subheading>Current Page: {currentPage}</Subheading>
      <SearchInput
        type="text"
        placeholder={LABEL.PAGE_BUTTON_SEARCH_PLACEHOLDER}
        onChange={(e) => {
          setCurrentPage(1);
          setSearchQuery(e.target.value);
        }}
        value={searchQuery}
      />
      <MovieListWrapper>
        {loading ? (
          <SkeletonGrid>
            {Array.from({ length: 12 }).map((_, idx) => (
              <SkeletonCard key={idx} />
            ))}
          </SkeletonGrid>
        ) : (
          <MovieList searchQuery={searchQuery} />
        )}
      </MovieListWrapper>
    </BodyContainer>
  );
};
