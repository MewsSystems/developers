/* Global imports */
import styled from "styled-components";
import { useLocation } from "wouter";
import { useEffect } from "react";
import { useInView } from "react-intersection-observer";

/* Local imports */
import { useMovies } from "../hooks/useMovies";
import { LoadingMessage } from "./ui/LoadingMessage";
import { Text } from "@components/ui/Layout";
import { motion } from "framer-motion";

/* Types  */

/* Local utility functions */
const container = {
  hidden: { opacity: 0 },
  show: {
    opacity: 1,
    transition: {
      staggerChildren: 0.1,
    },
  },
};

const listItem = {
  hidden: { opacity: 0 },
  show: { opacity: 1 },
};
/* Component definition */
export const Movies = ({ searchTerm = "" }: { searchTerm: string }) => {
  const { ref, inView } = useInView();
  const [, setLocation] = useLocation();
  const {
    data: movies,
    fetchNextPage,
    isLoading,
    isFetching,
    error,
    hasNextPage,
  } = useMovies(searchTerm);

  useEffect(() => {
    if (inView && hasNextPage) {
      fetchNextPage();
    }
  }, [inView, fetchNextPage, hasNextPage]);

  if (error) {
    return <div> Something went wrong!</div>;
  }

  return (
    <Container>
      {isLoading ? (
        <LoadingMessage />
      ) : (
        <ListScroll>
          <MovieContainer variants={container} initial="hidden" animate="show">
            {movies &&
              movies.pages &&
              movies.pages.map((page, index) =>
                page.results.length > 0 ? (
                  page.results.map((movie) => {
                    return (
                      <MovieItem
                        variants={listItem}
                        key={movie.id}
                        onClick={() => setLocation(`/movies/${movie.id}`)}
                        ref={ref}
                      >
                        <MoviePoster>
                          <MoviePosterImage
                            src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
                            alt={movie.title}
                          />
                        </MoviePoster>
                        <Text color="white">{movie.title}</Text>
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
          {isFetching && <LoadingMessage text="Loading more content..." />}
        </ListScroll>
      )}
    </Container>
  );
};

const Container = styled(motion.div)`
  position: relative;
  display: flex;
  flex-direction: column;
`;
const ListScroll = styled.div`
  overflow-y: scroll;
  height: 100vh;
  position: relative;
`;
const MovieContainer = styled(motion.ul)`
  display: flex;
  flex-wrap: wrap;
  list-style: none;
  justify-content: center;
  flex-direction: row;
`;

const MoviePosterImage = styled.img`
  border-width: 8px;
  max-width: 300px;
  border-radius: 1.5rem;
  @media (max-width: 600px) {
    max-width: 100%;
  }

  &:hover {
    box-shadow: 5px 2px 10px black;
  }
`;

const MovieItem = styled(motion.li)`
  display: flex;
  flex-direction: column;
  list-style: none;
  text-align: center;
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
