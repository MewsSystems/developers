import Flex, { FlexItem } from '../common/Flex';
import Genres from '../Genres';
import Rating from '../Rating';
import FlexIconText from '../common/FlexIconText';
import { CalendarAlt, Clock } from '@styled-icons/fa-solid';
import { MovieDetailContainer, MovieDetailTitle } from './styled';
import { MovieDetail } from '../../services/tmdbApi';
import { useDocumentTitle } from '../../hooks';
import { splitDate } from '../../utils';
import { useMediaQuery } from 'react-responsive';
import theme from '../../theme/theme';
import MoviePoster from '../MoviePoster';

const posterSizes = {
  small: {
    width: 154,
    height: 231,
  },
  large: {
    width: 342,
    height: 513,
  },
};

interface MovieProps {
  movie: MovieDetail;
}

function Movie({ movie }: MovieProps) {
  const [releaseYear] = splitDate(movie.release_date);
  const isLargeScreen = useMediaQuery({ minWidth: theme.breakPoints.mobileL });
  const posterSize = isLargeScreen ? posterSizes.large : posterSizes.small;

  useDocumentTitle(`${movie.title} | Movie Search`);

  return (
    <MovieDetailContainer padding="2rem">
      <MovieDetailTitle as="h1">
        {movie.title} <small>({releaseYear})</small>
      </MovieDetailTitle>

      <Genres genres={movie.genres} />

      <Flex justifyContent="space-between" gap="2rem 4rem">
        <FlexItem noShrink>
          <MoviePoster
            posterPath={movie.poster_path}
            alt={movie.title}
            width={posterSize.width}
            height={posterSize.height}
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

export default Movie;
