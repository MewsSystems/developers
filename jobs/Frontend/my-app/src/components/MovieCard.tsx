import styled from 'styled-components';
import miniposterplaceholder from './../assets/miniposterplaceholder.webp';

const StyledMovieCard = styled.div`
  display: flex;
  flex-flow: column nowrap;
  max-width: fit-content;
  padding: 0.5rem;
  align-items: center;
  font-size: 0.875rem;
`;

const StyledMovieCardPoster = styled.img`
  border-radius: 10px;
  max-width: 150px;
`;

const StyledMovieCardPlainText = styled.p`
  margin: 0;
`;

const StyledMovieCardTitle = styled(StyledMovieCardPlainText)`
  font-weight: 700;
`;

export const MovieCard = () => {
  return (
    <StyledMovieCard>
      <StyledMovieCardPoster src={miniposterplaceholder} alt="Movie poster" />
      <StyledMovieCardTitle>Movie name</StyledMovieCardTitle>
      <StyledMovieCardPlainText>Rating</StyledMovieCardPlainText>
      <StyledMovieCardPlainText>Release date</StyledMovieCardPlainText>
    </StyledMovieCard>
  );
};
