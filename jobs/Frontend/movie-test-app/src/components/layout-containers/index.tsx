import styled from 'styled-components';

const RowCenteredContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: row;
`;

const ColumnCenteredContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
`;

const MoviesGridContainer = styled.div`
  display: grid;
  height: 100vh;
  margin: 1rem;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr;
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

export { RowCenteredContainer, ColumnCenteredContainer, MoviesGridContainer };