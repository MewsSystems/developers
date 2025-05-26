import { Movie } from '@movie/types/movie';
import { Poster, MovieInfo, Title, MovieDetails, DetailItem, ReleaseDate, Rating, Overview } from './movie-content.styled';

interface MovieContentProps {
  movie: Movie;
}

const formatRuntime = (minutes: number): string => {
  const hours = Math.floor(minutes / 60);
  const remainingMinutes = minutes % 60;
  return `${hours}h ${remainingMinutes}m`;
};

const formatPopularity = (popularity: number): string => {
  return popularity.toFixed(1);
};

const formatLanguage = (language: string): string => {
  return language.toUpperCase();
};

export const MovieContent: React.FC<MovieContentProps> = ({ movie }) => {
  return (
    <>
      <Poster 
        src={`https://image.tmdb.org/t/p/w500${movie.posterPath}`} 
        alt={movie.title}
      />
      <MovieInfo>
        <Title>{movie.title}</Title>
        <MovieDetails>
          {movie.language && (
            <DetailItem>
              {formatLanguage(movie.language)}
            </DetailItem>
          )}
          {movie.runtime && (
            <DetailItem>
              {formatRuntime(movie.runtime)}
            </DetailItem>
          )}
          <DetailItem>
            Popularity: {formatPopularity(movie.popularity)}
          </DetailItem>
        </MovieDetails>
        <ReleaseDate>Release Date: {movie.releaseDate}</ReleaseDate>
        <Rating>
          Rating: {movie.voteAverage.toFixed(1)}/10
        </Rating>
        <Overview>{movie.overview}</Overview>
      </MovieInfo>
    </>
  );
}; 