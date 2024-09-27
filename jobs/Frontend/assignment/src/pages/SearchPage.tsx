import { useEffect, useState } from "react";
import styled from "styled-components";
import { BottomBar, Input, MovieCard, Pagination, Typography } from "../components";
import SearchIcon from "@material-ui/icons/Search";
import { Search, Movie } from "tmdb-ts";
import { useDebounce } from "use-debounce";
import { useGenres, useScrollToTop } from "@/hooks";
import { tmdbClient } from "@/tmdbClient";

const StyledWrapper = styled.div`
  max-width: 100vw;
  min-height: 100vh;

  padding: 23px 23px 100px 23px;

  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 16px;

  background-color: ${({ theme }) => theme.colors.surface.main};
`;

const CardsWrapper = styled.div`
  width: 100%;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(264px, 1fr));
  gap: 16px;
`;

const EmptyPageWrapper = styled.div`
  max-width: 400px;

  position: absolute;
  top: 50%;
  transform: translate(0, -50%);

  text-align: center;
  color: ${({ theme }) => theme.colors.surface.onVariant};
`;

export function SearchPage() {
  const [searchQuery, setSearchQuery] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [searchResults, setSearchResults] = useState<Search<Movie> | null>(null);

  const { getGenreNameById } = useGenres();
  useScrollToTop(currentPage);

  const [debouncedSearchQuery] = useDebounce(searchQuery, 500);

  useEffect(() => {
    setCurrentPage(1);
  }, [debouncedSearchQuery]);

  useEffect(() => {
    if (!debouncedSearchQuery) {
      setSearchResults(null);
      setCurrentPage(1);
      return;
    }

    tmdbClient.search.movies({ query: debouncedSearchQuery, page: currentPage }).then(res => {
      setSearchResults(res);
    });
  }, [debouncedSearchQuery, currentPage]);

  return (
    <StyledWrapper>
      <Input label="Search your movie" value={searchQuery} onChange={setSearchQuery} />
      {searchQuery && (
        <CardsWrapper>
          {searchResults?.results.map(movie => (
            <MovieCard
              key={movie.id}
              movie={movie}
              genres={movie.genre_ids.map(id => getGenreNameById(id))}
            />
          ))}
        </CardsWrapper>
      )}
      {!searchQuery && (
        <EmptyPageWrapper>
          <SearchIcon style={{ width: 200, height: 200 }} />
          <Typography variant="displaySmall">Type a keyword to start searching</Typography>
        </EmptyPageWrapper>
      )}
      <BottomBar
        leftChildren={
          searchResults?.total_pages && (
            <Pagination
              currentPage={currentPage}
              onChange={setCurrentPage}
              totalPages={searchResults?.total_pages || 1}
            />
          )
        }
      />
    </StyledWrapper>
  );
}
