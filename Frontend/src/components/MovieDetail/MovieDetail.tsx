import { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { LoadingState, ErrorState } from '../EmptyState';
import MoviePoster from '../MoviePoster';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { fetchMovieDetails } from '../../redux/movieReducer';
import { MovieParams } from '../../services/tmdbApi';
import { splitDate } from '../../utils';
import Flex, { FlexItem } from '../common/Flex';
import Genres from '../Genres';
import Rating from '../Rating';
import FlexIconText from '../common/FlexIconText';
import { CalendarAlt, Clock } from '@styled-icons/fa-solid';
import { MovieDetailContainer, MovieTitle } from './styled';

function MoviePage() {
  const dispatch = useAppDispatch();
  const { movieId } = useParams<MovieParams>();
  const { isLoading, error, timestamp, ...movie } = useAppSelector(
    (state) => state.movie
  );
  const [releaseYear] = splitDate(movie.release_date);

  useEffect(() => {
    dispatch(fetchMovieDetails({ movieId }));
  }, [dispatch, movieId]);

  useEffect(() => {
    if (movie.title) {
      document.title = [movie.title, 'Movie Search'].join(' | ');
    }
  }, [movie]);

  if (isLoading) {
    return <LoadingState title="Loading movie..." />;
  }

  if (error) {
    return (
      <ErrorState title="Movie Not Found">
        Type the name of the movie in the search box above
      </ErrorState>
    );
  }

  return (
    <MovieDetailContainer padding="2rem">
      <MovieTitle as="h1">
        {movie.title} <small>({releaseYear})</small>
      </MovieTitle>

      <Genres genres={movie.genres} />

      <Flex justifyContent="space-between" gap="2rem 4rem">
        <FlexItem noShrink>
          <MoviePoster
            posterPath={movie.poster_path}
            alt={movie.title}
            width={342}
            height={513}
          />
        </FlexItem>
        <FlexItem basis="50%" grow={1}>
          <Flex alignItems="center" gap="1em" style={{ marginBottom: '1rem' }}>
            <Rating
              voteAverage={movie.vote_average}
              voteCount={movie.vote_count}
            />

            <FlexIconText icon={Clock} title="Duration">
              <Flex flexDirection="column">
                <strong>Duration</strong>
                {movie.runtime} min.
              </Flex>
            </FlexIconText>

            <FlexIconText icon={CalendarAlt} title="Release Date">
              <Flex flexDirection="column">
                <strong>Release date</strong>
                {movie.release_date}
              </Flex>
            </FlexIconText>
          </Flex>

          <div>
            <strong>Overview</strong>
            <p>{movie.overview}</p>
          </div>

          {movie.title !== movie.original_title && (
            <div>
              <strong>Original title</strong>
              <p>{movie.original_title}</p>
            </div>
          )}
        </FlexItem>
      </Flex>
    </MovieDetailContainer>
  );
}

export default MoviePage;
