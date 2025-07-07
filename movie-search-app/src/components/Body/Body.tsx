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
    searchMoviesByQuery,
    searchQuery,
    setSearchQuery,
    setCurrentPage,
    currentPage,
    error,
  } = useMovieContext();

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
    if (searchQuery && searchQuery.trim() !== "") {
      searchMoviesByQuery(searchQuery, currentPage);
    } else if (searchQuery === "" || searchQuery === undefined) {
      // If no search query, fetch popular movies default list
      searchMovies();
    }
  }, [currentPage, searchQuery]);

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
          searchMoviesByQuery(e.target.value, currentPage);
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
