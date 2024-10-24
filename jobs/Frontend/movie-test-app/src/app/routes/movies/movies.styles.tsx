import styled from 'styled-components';

const MoviesGridContainer = styled.div`
  display: grid;
  height: 100vh;
  margin: 1rem;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr;
  grid-template-rows: 1fr 1fr 1fr;
  grid-gap: 1rem;
  transition: all 0.25s ease-in-out;
  color: white;
  @media screen and (max-width: 1000px) {
    grid-template-columns: 1fr 1fr 1fr 1fr;
  }
  @media screen and (max-width: 800px) {
    grid-template-columns: 1fr 1fr 1fr;
  }
  @media screen and (max-width: 600px) {
    grid-template-columns: 1fr 1fr;
  }
  @media screen and (max-width: 450px) {
    grid-template-columns: 1fr;
  }
`;

export { MoviesGridContainer };
