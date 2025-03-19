import styled from 'styled-components';
import miniposterplaceholder from './../assets/miniposterplaceholder.webp';

const StyledSearchResultCard = styled.div`
  display: flex;
  flex-flow: column nowrap;
  padding: 0.5rem;
`;

const StyledMiniMoviePoster = styled.img`
  border-radius: 10px;
  max-width: 150px;
`;

export const SearchResultCard = () => {
  return (
    <StyledSearchResultCard>
      <StyledMiniMoviePoster src={miniposterplaceholder} alt="Movie poster" />
      <h4>Movie name</h4>
      <p>Rating</p>
      <p>Release date</p>
    </StyledSearchResultCard>
  );
};
