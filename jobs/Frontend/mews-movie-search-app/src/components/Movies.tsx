/* Global imports */
import { useMovies } from "../hooks/useMovies";
import styled from "styled-components";
import { useLocation } from "wouter";
import { LoadingMessage } from "./ui/LoadingMessage";
import { UIEvent } from "react";
import { Text } from "@components/ui/Layout";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const Movies = ({ searchTerm = "" }: { searchTerm: string }) => {
  const [, setLocation] = useLocation();
  const {
    data: movies,
    fetchNextPage,
    isLoading,
    isFetching,
    error,
  } = useMovies(searchTerm);

  const handleScroll = (e: UIEvent<HTMLElement>) => {
    const { scrollTop, clientHeight, scrollHeight } = e.currentTarget;

    if (scrollHeight - Math.ceil(scrollTop) === clientHeight) {
      fetchNextPage();
    }
  };

  if (error) {
    return <div> Something went wrong!</div>;
  }

  return (
    <Container>
      {isLoading || isFetching ? (
        <LoadingMessage />
      ) : (
        <ListScroll onScroll={handleScroll}>
          <MovieContainer>
            {movies &&
              movies.pages &&
              movies.pages.map((page, index) =>
                page.results.length > 0 ? (
                  page.results.map((movie) => {
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
                  })
                ) : (
                  <NoResults key={`no-results-${index}`}>
                    <Text size="xl" color="#FFFFFF">
                      No results ffound
                    </Text>
                  </NoResults>
                )
              )}
          </MovieContainer>
        </ListScroll>
      )}
    </Container>
  );
};

const Container = styled.div`
  position: relative;
  display: flex;
  flex-direction: column;
`;
const ListScroll = styled.div`
  overflow-y: scroll;
  height: 100vh;
  position: relative;
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

  &:hover {
    box-shadow: 5px 2px 10px black;
  }
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
`;
