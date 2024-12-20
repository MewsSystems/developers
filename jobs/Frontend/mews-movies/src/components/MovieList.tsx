import styled from "styled-components";
import MovieCard from "./MovieCard";
import { Movie } from "../types/MovieInterfaces";

const List = styled.div`
  margin: 2rem auto;
  max-width: 90%;
  display: grid;
  grid-template-columns: 1fr;
  gap: 2rem;

  @media (min-width: 400px) {
    grid-template-columns: repeat(2, 1fr);
  }

  @media (min-width: 768px) {
    grid-template-columns: repeat(3, 1fr);
  }

  @media (min-width: 992px) {
    grid-template-columns: repeat(4, 1fr);
  }

  @media (min-width: 1200px) {
    grid-template-columns: repeat(5, 1fr);
  }
`;

interface MovieListProps {
  movies: Movie[];
}

const MovieList: React.FC<MovieListProps> = ({ movies }) => {
  return (
    <List>
      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </List>
  );
};

export default MovieList;
