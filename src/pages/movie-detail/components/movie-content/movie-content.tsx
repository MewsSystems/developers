import { Movie } from '@movie/types/movie';
import { Poster, MovieInfo, Title, Overview } from './movie-content.styled';
import { MovieDetails } from '../movie-details/movie-details';

interface MovieContentProps {
  movie: Movie;
}

export const MovieContent: React.FC<MovieContentProps> = ({ movie }) => {
  return (
    <>
      <Poster 
        src={`https://image.tmdb.org/t/p/w500${movie.posterPath}`} 
        alt={movie.title}
      />
      <MovieInfo>
        <Title>{movie.title}</Title>
        <MovieDetails movie={movie} />
        <Overview>{movie.overview}</Overview>
      </MovieInfo>
    </>
  );
}; 