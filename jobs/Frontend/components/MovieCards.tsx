"use client";

import styled from "styled-components";
import MovieCard from "./MovieCard";
import { MovieSearch } from "@/interfaces/movie";

const Container = styled.div`
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
`;

interface MovieCardsProps {
  movies: MovieSearch[];
}

const MovieCards = ({ movies }: MovieCardsProps) => {
  return (
    <Container>
      {movies.map((movie, index) => (
        <MovieCard
          key={index}
          id={movie.id}
          posterUrl={movie.posterUrl}
          releaseDate={movie.releaseDate}
          title={movie.title}
        />
      ))}
    </Container>
  );
};

export default MovieCards;
