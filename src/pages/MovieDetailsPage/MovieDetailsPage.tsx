import {useQuery, useQueryClient} from '@tanstack/react-query';
import {format} from 'date-fns';
import {useParams} from 'react-router-dom';
import {API_STATUS_MESSAGE, ERRORS_BY_HTTP_STATUS} from '../../api/movieApi/constants';
import {fetchMovieDetails} from '../../api/movieApi/endpoints/fetchMovieDetails';
import type {Movie} from '../../api/movieApi/types';
import {getErrorFallbackMessage} from '../../api/movieApi/utils/getErrorFallbackMessage';
import type {ApiErrorResponseDetails} from '../../api/movieApi/utils/types';
import ApiErrorScreen from '../../app/components/ApiErrorScreen/ApiErrorScreen';
import MovieCover from '../common/MovieCover/MovieCover';
import PopcornLoader from '../common/PopcornLoader/PopcornLoader';
import GoBackLink from './components/GoBackLink/GoBackLink';
import {
  Content,
  LoadingOverlay,
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
} from './styled';

export default function MovieDetailsPage() {
  const queryClient = useQueryClient();
  const {id} = useParams<{id: string}>();
  const {
    data: movie,
    error,
    isLoading,
  } = useQuery<Movie, ApiErrorResponseDetails>({
    queryKey: ['movie', id],
    queryFn: () => fetchMovieDetails(id!),
    enabled: Boolean(id),
  });

  if (isLoading) {
    return (
      <MovieDetailsPageContainer>
        <GoBackLink />
        <LoadingOverlay>
          <PopcornLoader />
        </LoadingOverlay>
      </MovieDetailsPageContainer>
    );
  }

  if (!movie || error) {
    return (
      <ApiErrorScreen
        errorMessage={
          error
            ? getErrorFallbackMessage({status: error.status, message: error.message})
            : ERRORS_BY_HTTP_STATUS[404][API_STATUS_MESSAGE.RESOURCE_NOT_FOUND]
        }
        onReset={() => {
          queryClient.clear();
        }}
      />
    );
  }

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
              <MetadataValue>{format(movie.release_date, 'dd MMM yyyy')}</MetadataValue>
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
                <MetadataValue>
                  {movie.genres.map((genre: {name: string}) => genre.name).join(', ')}
                </MetadataValue>
              </MetadataItem>
            )}
          </MetadataInfo>

          <Overview>{movie.overview}</Overview>

          {movie.tagline && (
            <MetadataItem>
              <MetadataLabel>Tagline</MetadataLabel>
              <MetadataValue>{movie.tagline}</MetadataValue>
            </MetadataItem>
          )}
        </MovieInfo>
      </Content>
    </MovieDetailsPageContainer>
  );
}

MovieDetailsPage.displayName = 'MovieDetailsPage';
