import styled from "styled-components";
import { Input, MovieCard, Pagination } from "../components";
import { useEffect, useMemo, useState } from "react";
import { TMDB, Search, Movie } from "tmdb-ts";
import { useDebounce } from "use-debounce";

const IMG_BASE_PATH = "https://image.tmdb.org/t/p/w300";
const tmdbClient = new TMDB(process.env.TMDB_ACCESS_TOKEN as string);

const StyledWrapper = styled.div`
  max-width: 100vw;
  min-height: 100vh;

  padding: 23px;

  display: flex;
  flex-direction: column;
  gap: 16px;

  background-color: ${({ theme }) => theme.colors.surface.main};
`;

const CardsWrapper = styled.div`
  max-width: 1130px;
  margin: 0 auto;

  width: 95%;
  display: grid;
  justify-content: center;
  grid-template-columns: repeat(auto-fill, minmax(264px, 1fr));
  gap: 16px;
`;

export function Search() {
  const [searchQuery, setSearchQuery] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [searchResults, setSearchResults] = useState<Search<Movie> | null>(null);

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
      <Input label="Seach your movie" value={searchQuery} onChange={setSearchQuery} />
      {searchQuery && (
        <>
          <CardsWrapper>
            {searchResults?.results.map(movie => {
              return (
                <MovieCard
                  imgPath={movie.poster_path ? IMG_BASE_PATH + movie.poster_path : null}
                  key={movie.id}
                  id={movie.id}
                  title={movie.title}
                  description={movie.overview}
                  releaseDate={movie.release_date}
                  rating={movie.vote_average}
                  genres={movie.genre_ids.map(id => genresMap.get(id) as string)}
                />
              );
            })}
          </CardsWrapper>
          <Pagination
            currentPage={currentPage}
            onChange={setCurrentPage}
            totalPages={searchResults?.total_pages || 1}
          />
        </>
      )}
      {!searchQuery && <p>Type </p>}
      <Pagination
        currentPage={currentPage}
        onChange={setCurrentPage}
        totalPages={searchResults?.total_pages || 1}
      />
    </StyledWrapper>
  );
}
