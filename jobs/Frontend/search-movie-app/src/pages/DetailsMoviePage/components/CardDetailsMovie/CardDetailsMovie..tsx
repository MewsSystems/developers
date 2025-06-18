import { AdultBadge, Image, ReleaseDate, Vote } from '../../../../components';
import { FILE_SIZE_DETAILS_MOVIE } from '../../../ListMoviesPage/constants';
import type { CardDetailsMovieProps } from '../../../types';
import {
  StyledCardMovieDetailsTitle,
  StyledCardMovieDetailsWrapper,
  StyledCardMovieDetailsTagline,
  StyledCardMovieDetailsOverview,
  StyledCardMovieDetailsGenreItem,
  StyledCardMovieDetailsContent,
  StyledCardMovieDetailsGenreWrapper,
  StyledCardMovieImageWrapper,
} from './CardDetailsMovie.styles';

export const CardDetailsMovie: React.FC<CardDetailsMovieProps> = ({ data }) => {
  const {
    isAdult,
    id,
    imageURL,
    mobileImageURL,
    genres,
    releaseDate,
    tagline,
    title,
    voteAverage,
    voteTotalCount,
    overview,
  } = data;

  return (
    <StyledCardMovieDetailsWrapper key={id}>
      <StyledCardMovieImageWrapper>
        <Image
          fileSize={FILE_SIZE_DETAILS_MOVIE}
          imageURL={imageURL}
          title={title}
          mobileImageURL={mobileImageURL}
          rounded
        />
      </StyledCardMovieImageWrapper>
      <StyledCardMovieDetailsContent>
        <StyledCardMovieDetailsTitle>{title}</StyledCardMovieDetailsTitle>
        <StyledCardMovieDetailsTagline>{tagline}</StyledCardMovieDetailsTagline>

        <StyledCardMovieDetailsGenreWrapper>
          {genres?.map(genre => (
            <StyledCardMovieDetailsGenreItem key={genre.id}>
              {genre.name}
            </StyledCardMovieDetailsGenreItem>
          ))}
        </StyledCardMovieDetailsGenreWrapper>
        <StyledCardMovieDetailsOverview>{overview}</StyledCardMovieDetailsOverview>
        <div className="flex align-center- justify-start">
          <Vote voteAverage={voteAverage} voteTotalCount={voteTotalCount} />
          {releaseDate && <ReleaseDate releaseDate={releaseDate} />}
          {isAdult && <AdultBadge />}
        </div>
      </StyledCardMovieDetailsContent>
    </StyledCardMovieDetailsWrapper>
  );
};
