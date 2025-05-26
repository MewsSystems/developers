import { Movie } from '@movie/types/movie';
import { MovieDetailsContainer, DetailItem, ReleaseDate, Rating } from './movie-details.styled';
import { formatRuntime } from '@app/utils/format-runtime';
import { formatPopularity } from '@app/utils/format-population';
import { formatLanguage } from '@app/utils/format-language';

interface MovieDetailsProps {
  movie: Movie;
}

export const MovieDetails = ({ movie }: MovieDetailsProps) => {
  return (
    <MovieDetailsContainer>
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
      <ReleaseDate>Release Date: {movie.releaseDate}</ReleaseDate>
      <Rating>
        Rating: {movie.voteAverage.toFixed(1)}/10
      </Rating>
    </MovieDetailsContainer>
  );
}; 