import { useEffect, useMemo, useState } from "react";
import styled from "styled-components";
import { IconButton, Input, MovieCard, Pagination, Typography } from "../components";
import SearchIcon from "@material-ui/icons/Search";
import ThemeModeIcon from "@material-ui/icons/WbSunnyOutlined";
import { TMDB, Search, Movie } from "tmdb-ts";
import { useDebounce } from "use-debounce";
import { useDarkMode } from "@/hooks";

export const IMG_BASE_PATH = "https://image.tmdb.org/t/p/w300";
export const tmdbClient = new TMDB(process.env.TMDB_ACCESS_TOKEN as string);

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

const BottomBar = styled.div`
  position: fixed;
  bottom: 0;
  left: 0;

  display: flex;
  align-items: center;
  justify-content: space-between;

  width: 100%;
  padding: 12px 16px;

  backdrop-filter: blur(10px);
  border-top: 1px solid ${({ theme }) => theme.colors.outline.variant};

  &:before {
    content: " ";
    position: absolute;
    top: 0;
    left: 0;
    z-index: -1;

    width: 100%;
    height: 100%;
    opacity: 0.8;

    background-color: ${({ theme }) => theme.colors.surface.main};
  }
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

  const [debouncedSearchQuery] = useDebounce(searchQuery, 500);
  const { toggleDarkMode } = useDarkMode();

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
              imgPath={movie.poster_path ? IMG_BASE_PATH + movie.poster_path : null}
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
      <BottomBar>
        <div>
          {searchResults?.total_pages && (
            <Pagination
              currentPage={currentPage}
              onChange={setCurrentPage}
              totalPages={searchResults?.total_pages || 1}
            />
          )}
        </div>
        <IconButton size="large" onClick={toggleDarkMode}>
          <ThemeModeIcon />
        </IconButton>
      </BottomBar>
    </StyledWrapper>
  );
}
