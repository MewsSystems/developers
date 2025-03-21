import styled from 'styled-components';
import fallback_image from './../assets/image-load-failed.svg';

const StyledMovieCard = styled.div`
  display: flex;
  flex-flow: column nowrap;
  max-width: 200px;
  width: 100%;
  padding: 1rem;
  align-items: center;
  font-size: 0.875rem;
`;

const StyledMovieCardPoster = styled.img`
  border-radius: 10px;
  max-width: 150px;
`;

const StyledMovieCardPlainText = styled.p`
  width: 100%;
  margin: 0;
  text-wrap: pretty;
`;

const StyledMovieCardTitle = styled(StyledMovieCardPlainText)`
  font-weight: 700;
`;

interface MovieCardProps {
  poster: string;
  name: string;
  rating: string;
  release_date: string;
}

export const MovieCard = ({
  poster,
  name,
  rating,
  release_date,
}: MovieCardProps) => {
  return (
    <StyledMovieCard>
      <StyledMovieCardPoster
        src={
          poster ? `https://image.tmdb.org/t/p/w200/${poster}` : fallback_image
        }
        alt="Movie poster"
      />
      <StyledMovieCardTitle>{name}</StyledMovieCardTitle>
      <StyledMovieCardPlainText>{rating}</StyledMovieCardPlainText>
      <StyledMovieCardPlainText>{release_date}</StyledMovieCardPlainText>
    </StyledMovieCard>
  );
};
