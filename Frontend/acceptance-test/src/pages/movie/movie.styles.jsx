import styled from "styled-components";

export const Poster = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  background-image: url(${props => props.imageBg});
  background-repeat: no-repeat;
  background-size: contain;
  overflow: hidden;
  -webkit-filter: drop-shadow(0 1.5rem 4rem rgba(0, 0, 0, 0.15));
  filter: drop-shadow(0 1.5rem 4rem rgba(0, 0, 0, 0.15));
`;

export const MovieContainer = styled.div`
  background: $color-grey-light-1;
  display: grid;
  flex-wrap: wrap;
  height: 100vh;
  grid-template-columns: 1fr 2fr;
  grid-template-rows: 1fr;

  & .movie-details {
    font-size: 2.5rem;
    display: flex;
    justify-content: flex-start;
    align-items: center;
  }
`;
