import styled from 'styled-components';

export const MovieWrapper = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
  cursor: pointer;
`;

export const MovieTitle = styled.div`
  flex-grow: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
  margin-bottom: 5px;
`;

export const MoviePoster = styled.img`
  width: 100%;
`;