import {useQuery} from '@tanstack/react-query';
import {useParams} from 'react-router-dom';
import {fetchMovieDetails} from '../../api/fetchMovieDetails';
import LoadingCameraAnimation from '../MoviesListPage/components/LoadingCameraAnimation/LoadingCameraAnimation';
import {
  Content,
  MetadataInfo,
  MetadataItem,
  MetadataLabel,
  MetadataValue,
  MovieDetailsPageContainer,
  MovieInfo,
  Overview,
  PosterContainer,
  Rating,
  Title,
} from './MovieDetailsPage.styled';
import MovieCover from '../common/MovieCover/MovieCover.tsx';
import GoBackLink from './components/GoBackLink/GoBackLink';

export default function MovieDetailsPage() {
  const {id} = useParams<{id: string}>();
  const {data: movie, isLoading} = useQuery({
    queryKey: ['movie', id],
    queryFn: () => fetchMovieDetails(id!),
    enabled: Boolean(id),
  });

  if (isLoading) {
    return (
      <MovieDetailsPageContainer>
        <LoadingCameraAnimation />
      </MovieDetailsPageContainer>
    );
  }

  if (!movie) {
    return (
      <MovieDetailsPageContainer>
        <Title>Movie not found</Title>
        <GoBackLink />
      </MovieDetailsPageContainer>
    );
  }

  const formattedDate = new Date(movie.release_date).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  });

  return (
    <MovieDetailsPageContainer>
      <GoBackLink />
      <Content>
        <PosterContainer>
          <MovieCover poster_path={movie.poster_path} title={movie.title} />
        </PosterContainer>

        <MovieInfo>
          <Title>{movie.title}</Title>

          <MetadataInfo>
            <Rating $rating={movie.vote_average}>★ {movie.vote_average.toFixed(1)}</Rating>

            <MetadataItem>
              <MetadataLabel>Release Date</MetadataLabel>
              <MetadataValue>{formattedDate}</MetadataValue>
            </MetadataItem>

            {movie.runtime && (
              <MetadataItem>
                <MetadataLabel>Runtime</MetadataLabel>
                <MetadataValue>{movie.runtime} minutes</MetadataValue>
              </MetadataItem>
            )}

            {movie.genres && movie.genres.length > 0 && (
              <MetadataItem>
                <MetadataLabel>Genres</MetadataLabel>
                <MetadataValue>{movie.genres.map((genre) => genre.name).join(', ')}</MetadataValue>
              </MetadataItem>
            )}
          </MetadataInfo>

          <Overview>{movie.overview}</Overview>

          {movie.tagline && (
            <MetadataItem>
              <MetadataLabel>Tagline</MetadataLabel>
              <MetadataValue>"{movie.tagline}"</MetadataValue>
            </MetadataItem>
          )}
        </MovieInfo>
      </Content>
    </MovieDetailsPageContainer>
  );
}

MovieDetailsPage.displayName = 'MovieDetailsPage';
