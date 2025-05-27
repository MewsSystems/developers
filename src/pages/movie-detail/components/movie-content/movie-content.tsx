import { Movie } from '@movie/types/movie';
import {
  Poster,
  MovieInfo,
  Title,
  Overview,
  DetailItem,
  MovieDetailsContainer,
  Rating,
  ReleaseDate,
} from './movie-content.styled';
import { formatLanguage } from '@pages/movie-detail/utils/format-language';
import { formatPopularity } from '@pages/movie-detail/utils/format-population';
import { formatRuntime } from '@pages/movie-detail/utils/format-runtime';
import { IMAGE_BASE_URL } from '@app/const/image-base-url';

interface MovieContentProps {
  movie: Movie;
}
const posterUrl = `${IMAGE_BASE_URL}/w500`;

export const MovieContent = ({ movie }: MovieContentProps) => {
  return (
    <>
      <Poster src={`${posterUrl}${movie.posterPath}`} alt={movie.title} />
      <MovieInfo>
        <Title>{movie.title}</Title>
        <MovieDetailsContainer>
          {movie.language && <DetailItem>{formatLanguage(movie.language)}</DetailItem>}
          {movie.runtime && <DetailItem>{formatRuntime(movie.runtime)}</DetailItem>}
          <DetailItem>Popularity: {formatPopularity(movie.popularity)}</DetailItem>
          <ReleaseDate>Release Date: {movie.releaseDate}</ReleaseDate>
          <Rating>Rating: {movie.voteAverage.toFixed(1)}/10</Rating>
        </MovieDetailsContainer>
        <Overview>{movie.overview}</Overview>
      </MovieInfo>
    </>
  );
};
