import { useEffect, useMemo, useState } from "react";
import styled from "styled-components";
import { BottomBar, Input, MovieCard, Pagination, Typography } from "../components";
import SearchIcon from "@material-ui/icons/Search";
import { Search, Movie } from "tmdb-ts";
import { useDebounce } from "use-debounce";
import { useScrollToTop } from "@/hooks";
import { MEDIA_300_BASE_URL, tmdbClient } from "@/tmdbClient";

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
  position: relative;
  max-width: 1130px;

  width: 95%;
  display: grid;
  justify-content: center;
  grid-template-columns: repeat(auto-fill, minmax(264px, 1fr));
  gap: 16px;
`;

const Test = styled.div`
  max-width: 400px;

  position: absolute;
  top: 50%;
  transform: translate(0, -50%);

  text-align: center;
  color: ${({ theme }) => theme.colors.surface.onVariant};
`;

export function Search() {
  const [searchQuery, setSearchQuery] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [searchResults, setSearchResults] = useState<Search<Movie> | null>(null);

  useScrollToTop(currentPage);

  const [debouncedSearchQuery] = useDebounce(searchQuery, 500);

  const genresMap = useMemo(() => {
    const genresMap = new Map<number, string>();

    tmdbClient.genres
      .movies()
      .then(res => res.genres.forEach(genre => genresMap.set(genre.id, genre.name)));

    return genresMap;
  }, []);

  useEffect(() => {
    if (!debouncedSearchQuery) {
      setSearchResults(null);
      setCurrentPage(1);
      return;
    }

    tmdbClient.search
      .movies({ query: debouncedSearchQuery, page: currentPage })
      .then(res => setSearchResults(res));

    window.scrollTo({ top: 0, behavior: "smooth" });
  }, [debouncedSearchQuery, currentPage]);

  return (
    <StyledWrapper>
      <Input label="Search your movie" value={searchQuery} onChange={setSearchQuery} />
      {searchQuery && (
        <CardsWrapper>
          {searchResults?.results.map(movie => (
            <MovieCard
              imgPath={movie.poster_path ? MEDIA_300_BASE_URL + movie.poster_path : null}
              key={movie.id}
              id={movie.id}
              title={movie.title}
              description={movie.overview}
              releaseDate={movie.release_date}
              rating={movie.vote_average / 2}
              genres={movie.genre_ids.map(genreId => genresMap.get(genreId) || "Unknown")}
            />
          ))}
        </CardsWrapper>
      )}
      {!searchQuery && (
        <Test>
          <SearchIcon style={{ width: 200, height: 200 }} />
          <Typography variant="displaySmall">Type a keyword to start searching</Typography>
        </Test>
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
