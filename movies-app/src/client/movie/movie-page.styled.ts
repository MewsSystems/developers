import styled from 'styled-components';

export const MoviePageWrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
`;

export const ErrorWrapper = styled.div`
  color: var(--common-color-error);
  font-size: 16px;
`;

export const HomeButton = styled.div`
  margin-top: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 10px;
  background-color: #ffffff;
  cursor: pointer;
  border: 1px solid var(--common-color-blue-1);
  border-radius: 5px;

  &:hover {
    background-color: var(--common-color-blue-2);
  }
`;

export const MovieTitle = styled.h1`
  font-size: 24px;
  margin-bottom: 20px;
`;

export const MovieImageBlock = styled.div`
  width: 45%;
  text-align: right;
`;

export const MoviePoster = styled.img`
  max-width: 100%;
`;

export const MovieInfo = styled.div`
  display: flex;
  justify-content: center;
  width: 100%;
`;

export const MovieDetailsWrapper = styled.div`
  width: 55%;
  padding-left: 20px;
`;

export const MovieOverview = styled.div`
  margin-top: 20px;
`;