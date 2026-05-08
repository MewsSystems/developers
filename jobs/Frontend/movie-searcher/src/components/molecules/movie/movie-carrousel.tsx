import { useState } from "react";
import styled from "styled-components";
import { Movie } from "../../../models/tmdbModels";
import { MoviePosterCard } from "../../atoms/movie/movie-poster-card";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";

export const MovieCarrousel: React.FC<{ title: string; movies: Movie[] }> = ({
  title,
  movies,
}) => {
  const [scrollIndex, setScrollIndex] = useState(0);
  const visibleMovies = 6;

  const scrollLeft = () => {
    setScrollIndex((prev) => Math.max(prev - 1, 0));
  };

  const scrollRight = () => {
    setScrollIndex((prev) => Math.min(prev + 1, movies.length - visibleMovies));
  };

  return (
    <Container>
      <Title>{title}</Title>
      <CarouselWrapper>
        <ScrollButton
          onClick={scrollLeft}
          disabled={scrollIndex <= 0}
          position="left"
        >
          <KeyboardArrowLeftIcon fontSize="large" />
        </ScrollButton>
        <MoviesContainer>
          <MoviesRow scrollIndex={scrollIndex} visibleMovies={visibleMovies}>
            {movies.map((movie, index) => (
              <MovieItem key={index}>
                <MoviePosterCard movie={movie} info />
              </MovieItem>
            ))}
          </MoviesRow>
        </MoviesContainer>
        <ScrollButton
          onClick={scrollRight}
          disabled={scrollIndex >= movies.length - visibleMovies}
          position="right"
        >
          <KeyboardArrowRightIcon fontSize="large" />
        </ScrollButton>
      </CarouselWrapper>
    </Container>
  );
};

const Container = styled.div`
  margin: 2rem 1rem;
  font-size: 1.875rem;
  font-weight: 600;
`;

const Title = styled.h1`
  color: #c4ab9c;
`;

const CarouselWrapper = styled.div`
  position: relative;
`;

const ScrollButton = styled.button<{ position: "left" | "right" }>`
  display: flex;
  position: absolute;
  top: 50%;
  ${(props) => (props.position === "left" ? "left: 0;" : "right: 0;")}
  transform: translateY(-50%);
  background: rgba(0, 0, 0, 0.6);
  color: white;
  padding: 0.5rem;
  border-radius: 50%;
  z-index: 10;
  transition: background 0.3s ease-in-out;
  opacity: ${(props) => (props.disabled ? "0.4" : "1")};
  cursor: ${(props) => (props.disabled ? "not-allowed" : "pointer")};

  &:hover {
    background: ${(props) => !props.disabled && "black"};
  }
`;

const MoviesContainer = styled.div`
  overflow: hidden;
`;

const MoviesRow = styled.div<{ scrollIndex: number; visibleMovies: number }>`
  display: flex;
  transition: transform 0.3s ease-in-out;
  transform: translateX(
    -${(props) => props.scrollIndex * (100 / props.visibleMovies)}%
  );
`;

const MovieItem = styled.div`
  flex: 0 0 20%;
  margin-right: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
`;
