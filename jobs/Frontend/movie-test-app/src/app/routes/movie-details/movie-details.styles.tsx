import styled from 'styled-components';

const MovieDetailsContainer = styled.div`
  display: flex;
  justify-content: start;
  align-items: center;
  flex-direction: column;
  color: ${(props) => props.theme.primary};
  background: linear-gradient(
    to top,
    ${(props) => props.theme.secondary} 0%,
    white 100%
  ); /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
  height: 100vh;
  white-space: break-spaces;

  h4 {
    top: 0;
    width: 6rem;
  }
`;

const ImageContainer = styled.img`
  width: 100%;
  height: 100%;
  border-radius: 1rem;
`;

const OverviewContainer = styled.div`
  width: 60%;
  margin: 1rem;
  @media screen and (max-width: 800px) {
    width: 80%;
  }
  @media screen and (max-width: 600px) {
    width: 95%;
  }
`;

const DetailsContainer = styled.div`
  display: flex;
  justify-content: start;
  align-items: self-start;
  flex-direction: column;
  margin-left: 1rem;
  height: 100%;
`;

const MovieDetailGrid = styled.div`
  display: grid;
  grid-template-columns: 5fr 5fr;
  width: 80%;
  @media screen and (max-width: 1400px) {
    grid-template-columns: 5fr 3fr;
    width: 70%;
  }
  gap: 1rem;

  @media screen and (max-width: 1200px) {
    grid-template-columns: 1fr;
    width: 50%;
  }
  @media screen and (max-width: 1000px) {
    width: 60%;
  }
  @media screen and (max-width: 800px) {
    width: 80%;
  }
  @media screen and (max-width: 600px) {
    width: 95%;
  }
`;
const RowCenteredContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: row;
`;

export {
  MovieDetailsContainer,
  ImageContainer,
  OverviewContainer,
  DetailsContainer,
  MovieDetailGrid,
  RowCenteredContainer,
};
