/* Global imports */
import * as React from "react";
import { useMovies } from "./useMovies";
import styled from "styled-components";
import { useLocation } from "wouter";
import { Movie } from "./types/movies";
import { LoadingMessage } from "./LoadingMessage";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const Movies = ({ searchTerm }: { searchTerm?: string }) => {
  const [page, setPage] = React.useState(1);
  const [, setLocation] = useLocation();
  const { movies, isLoading } = useMovies({ searchTerm, page });

  const handlePageChange = (current: number) => {
    if (movies) {
      if (current > 0 && current <= movies?.total_pages && current !== page)
        setPage(current);
    }
  };

  return (
    <Container>
      {isLoading ? (
        <LoadingMessage />
      ) : movies?.results && movies?.results.length > 0 ? (
        <ListScroll>
          <Pagination
            currentPage={movies.page}
            totalPages={movies.total_pages}
            onNext={() => handlePageChange(movies.page + 1)}
            onPrev={() => handlePageChange(movies.page - 1)}
          />
          <MovieContainer>
            {movies.results.map((movie: Movie) => {
              return (
                <MovieItem
                  key={movie.id}
                  onClick={() => setLocation(`/movies/${movie.id}`)}
                >
                  <MoviePoster>
                    <MoviePosterImage
                      src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
                      alt={movie.title}
                    />
                  </MoviePoster>
                </MovieItem>
              );
            })}
          </MovieContainer>
        </ListScroll>
      ) : (
        <NoResults>
          <span>No results are found!</span>
        </NoResults>
      )}
    </Container>
  );
};

function Pagination({
  currentPage,
  totalPages,
  onNext,
  onPrev,
}: {
  currentPage: number;
  totalPages: number;
  onNext: () => void;
  onPrev: () => void;
}) {
  if (totalPages === 1) return null;

  return (
    <PaginationContainer>
      <PaginationButton disabled={currentPage === 1} onClick={onPrev}>
        {`< Prev`}
      </PaginationButton>
      <PaginationButton disabled={currentPage === totalPages} onClick={onNext}>
        {`Next > `}
      </PaginationButton>
    </PaginationContainer>
  );
}
const Container = styled.div`
  position: relative;
  display: flex;
  flex-direction: column;
  height: 100vh;
`;
const ListScroll = styled.div`
  overflow-y: auto;
`;
const PaginationContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 1rem;
  gap: 0.5rem;
`;
const PaginationButton = styled.button`
  border-radius: 4px;
  border: none;
  padding: 0.5rem 1rem;
`;
const MovieContainer = styled.ul`
  display: flex;
  flex-wrap: wrap;
  list-style: none;
`;

const MovieItem = styled.li`
  display: flex;
  flex-direction: column;
  list-style: none;
  text-align: center;
`;
const MoviePosterImage = styled.img`
  border-width: 8px;
  max-width: 300px;
  border-radius: 1.5rem;
`;
const MoviePoster = styled.div`
  padding: 1rem;
`;

const NoResults = styled.div`
  padding: 1rem;
  justify-content: center;
  align-items: center;
  height: 100vh;
  width: 100%;
  display: flex;

  & > span {
    font-size: 2rem;
    color: #e3fef7;
  }
`;
