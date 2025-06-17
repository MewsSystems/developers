import styled, { keyframes } from 'styled-components';
import { StyledCardMovieContainer } from '../CardMovie/CardMovie.styles';

const shimmer = keyframes`
  0% {
    background-position: -400px 0;
  }
  100% {
    background-position: 400px 0;
  }
`;

const SkeletonBlock = styled.div`
  background: linear-gradient(90deg, #eeeeee 25%, #dddddd 50%, #eeeeee 75%);
  background-size: 800px 100%;
  animation: ${shimmer} 1.2s infinite;
  border-radius: 4px;
`;

export const SkeletonCard = styled(StyledCardMovieContainer)`
  background-color: ${({ theme }) => theme.colors.surface};
`;

export const SkeletonImage = styled(SkeletonBlock)`
  width: 250px;
  height: 360px;
  border-radius: 8px 8px 0 0;
`;

export const SkeletonTitle = styled(SkeletonBlock)`
  height: 20px;
  width: 80%;
  margin: 8px;
`;

export const SkeletonBottomInfo = styled.div`
  display: flex;
  justify-content: space-between;
  padding: 10px;
`;

export const SkeletonText = styled(SkeletonBlock)`
  height: 16px;
  width: 30%;
`;
