import styled from "styled-components";

export const StyledMovieCardImage = styled.img`
  width: 100%;
  height: auto;
  border-radius: 0.5rem;
  transition: transform 0.2s ease;
`;

export const StyledMovieImgPlaceholder = styled.div`
  width: 100%;
  height: 300px;
  display: flex;
  justify-content: center;
  align-items: center;
  color: white;
  font-size: 1.5rem;
  text-align: center;
  background: linear-gradient(60deg, #29323c 0%, #485563 100%);
  border-radius: 0.5rem;
  transition: transform 0.2s ease;
`;

export const StyledMovieCardContainer = styled.div`
  width: 200px;
  padding: 10px;
  margin: 10px;
  cursor: pointer;

  &:hover {
    & > ${StyledMovieCardImage} {
      transform: scale(1.2);
    }
  }

  &:hover {
    & > ${StyledMovieImgPlaceholder} {
      transform: scale(1.2);
    }
  }
`;
