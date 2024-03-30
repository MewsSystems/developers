import styled from 'styled-components';

export const MoviesWrapper = styled.div`
  display: grid;
  width: 100%;
  row-gap: 10px;
  grid-template-columns: repeat(5, 20%);

  @media (min-width: 751px) and (max-width: 1000px) {
    grid-template-columns: repeat(4, 25%);
  }

  @media (min-width: 501px) and (max-width: 750px) {
    grid-template-columns: repeat(3, 33.3%);
  }

  @media (max-width: 500px) {
    grid-template-columns: repeat(2, 50%);
  }
`;

export const MovieItem = styled.div`
  transition: transform 200ms;

  &:hover {
    transform: scale(1.05);
  }
`;