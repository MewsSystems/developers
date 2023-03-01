import styled from "styled-components";

export const MoviePreviewLayout = styled.div`
  width: 200px;
  text-decoration: none;

  @media (max-width: 500px) {
    width: 150px;
  }
`;

export const MoviePreviewTitle = styled.h3`
  font-size: 1.4rem;
  text-align: left;
  color: #333;
`;

export const PosterPlaceholder = styled.div`
  width: 200px;
  height: 300px;
  background-color: #ccc;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  position: relative;

  @media (max-width: 500px) {
    width: 150px;
    height: 225px;
  }

  & img {
    max-width: 100%;
    max-height: 100%;
    position: relative;
    z-index: 10;
  }

  &::after {
    content: 'Loading...';
    z-index: 1;
    position: absolute;
    display: flex;
    align-self: center;
    justify-self: center;
    font-size: 1.2rem;
    color: #666;
    text-transform: uppercase;
  }
`;

export const NoPoster = styled.div`
  font-size: 1.2rem;
  color: #666;
  text-transform: uppercase;
  background: #ccc;
  padding: 2rem;
  position: relative;
  z-index: 10;
`;
