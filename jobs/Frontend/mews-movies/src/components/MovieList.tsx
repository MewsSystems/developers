import React, { useEffect, useState } from "react";
import styled from "styled-components";
import MovieCard from "./MovieCard";
import { Movie } from "../types/MovieInterfaces";

const List = styled.div<{ cardsPerRow: number }>`
  margin: 2rem auto;
  max-width: 90%;
  display: grid;
  grid-template-columns: repeat(${(props) => props.cardsPerRow}, 1fr);
  gap: 1rem;
`;

interface MovieListProps {
  movies: Movie[];
}

const MovieList: React.FC<MovieListProps> = ({ movies }) => {
  const [cardsPerRow, setCardsPerRow] = useState(5);

  useEffect(() => {
    const updateCardsPerRow = () => {
      const width = window.innerWidth;
      if (width >= 1200) setCardsPerRow(5);
      else if (width >= 992) setCardsPerRow(4);
      else if (width >= 768) setCardsPerRow(3);
      else setCardsPerRow(2);
    };

    window.addEventListener("resize", updateCardsPerRow);
    updateCardsPerRow();

    return () => {
      window.removeEventListener("resize", updateCardsPerRow);
    };
  }, []);

  const rows = Math.ceil(movies.length / cardsPerRow);
  const paginatedMovies = movies.slice(0, cardsPerRow * rows);

  return (
    <>
      <List cardsPerRow={cardsPerRow}>
        {paginatedMovies.map((movie) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </List>
    </>
  );
};

export default MovieList;
