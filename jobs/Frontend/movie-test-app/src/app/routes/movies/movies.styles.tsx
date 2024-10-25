import styled from 'styled-components';

const MoviesGridContainer = styled.div`
  display: grid;
  height: 100vh;
  margin: 3rem;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr;
  grid-template-rows: 1fr 1fr 1fr;
  grid-gap: 2rem;
  transition: all 0.25s ease-in-out;
  color: white;
  @media screen and (max-width: ${(props) => props.theme.breakpoints.largeDesktop}) {
    grid-template-columns: 1fr 1fr 1fr 1fr 1fr;
  }
  @media screen and (max-width: ${(props) => props.theme.breakpoints.desktop}) {
    grid-template-columns: 1fr 1fr 1fr 1fr;
  }
  @media screen and (max-width: ${(props) => props.theme.breakpoints.smallDesktop}) {
    grid-template-columns: 1fr 1fr 1fr;
  }
  @media screen and (max-width: ${(props) => props.theme.breakpoints.tablet}) {
    grid-template-columns: 1fr 1fr;
  }
  @media screen and (max-width: ${(props) => props.theme.breakpoints.mobile}) {
    grid-template-columns: 1fr;
    margin: 2rem;
  }
`;

export { MoviesGridContainer };
