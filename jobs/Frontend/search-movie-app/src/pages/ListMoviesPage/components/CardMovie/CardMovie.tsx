import {
  StyledCardMovieContainer,
  StyledCardMovieContent,
  StyledInfoBottom,
  StyledTitle,
} from './CardMovie.styles';
import { AdultBadge, Image, Vote, ReleaseDate } from '../../../../components';
import { FILE_SIZE_LIST_MOVIES } from '../../constants';
import type { CardMovieProps } from '../../../types';

export const CardMovie: React.FC<CardMovieProps> = ({ data, handleOnClick }) => {
  const { imageURL, isAdult, releaseDate, title, voteAverage, voteTotalCount } = data;

  return (
    <StyledCardMovieContainer
      role="button"
      onClick={handleOnClick}
      tabIndex={0}
      data-testid="card-movie"
    >
      <StyledCardMovieContent>
        <Image fileSize={FILE_SIZE_LIST_MOVIES} imageURL={imageURL} title={title} />
        <StyledInfoBottom>
          {voteTotalCount ? (
            <Vote voteAverage={voteAverage} voteTotalCount={voteTotalCount} />
          ) : null}
          {isAdult && <AdultBadge />}
          {releaseDate && <ReleaseDate releaseDate={releaseDate} />}
        </StyledInfoBottom>
      </StyledCardMovieContent>
      <StyledTitle>{title}</StyledTitle>
    </StyledCardMovieContainer>
  );
};
