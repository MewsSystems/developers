import { FaStar, FaCalendarAlt } from 'react-icons/fa';
import { TbRating18Plus } from 'react-icons/tb';
import {
  StyledAdultBadge,
  StyledCardMovieContainer,
  StyledCardMovieContent,
  StyledCardMovieImage,
  StyledInfoBottom,
  StyledReleaseDate,
  StyledTitle,
  StyledVote,
  StyledVoteNumber,
} from './CardMovie.styles';
import { getImageURL } from '../../../../utils/getImageURL';
import noImageAvailable from '../../../../assets/No_Image_Available.jpg';
import { parseReleaseDate, parseVoteAverage } from '../../utils';
import type { CardMovieProps } from '../../types';

export const CardMovie: React.FC<CardMovieProps> = ({ data, handleOnClick }) => {
  const { imageURL, isAdult, releaseDate, title, voteAverage, voteTotalCount } = data;
  const { type, vote } = parseVoteAverage(voteAverage);

  return (
    <StyledCardMovieContainer>
      <StyledCardMovieContent role="button" onClick={handleOnClick} tabIndex={0}>
        <StyledCardMovieImage
          src={imageURL ? getImageURL(imageURL) : noImageAvailable}
          alt={title}
          loading="lazy"
        />
        <StyledInfoBottom>
          {voteTotalCount ? (
            <StyledVote>
              <StyledVoteNumber $type={type}>
                <FaStar style={{ marginRight: 4 }} />
                {vote}
              </StyledVoteNumber>
              {`(${voteTotalCount})`}
            </StyledVote>
          ) : null}
          {isAdult && (
            <StyledAdultBadge title="+18">
              <TbRating18Plus />
            </StyledAdultBadge>
          )}
          {releaseDate && (
            <StyledReleaseDate>
              <FaCalendarAlt style={{ marginRight: 4 }} />
              {parseReleaseDate(releaseDate)}
            </StyledReleaseDate>
          )}
        </StyledInfoBottom>
      </StyledCardMovieContent>
      <StyledTitle>{title}</StyledTitle>
    </StyledCardMovieContainer>
  );
};
