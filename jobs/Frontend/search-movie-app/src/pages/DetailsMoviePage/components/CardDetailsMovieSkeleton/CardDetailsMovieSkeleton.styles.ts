import styled, { keyframes } from 'styled-components';
import { StyledCardMovieDetailsWrapper } from '../CardDetailsMovie/CardDetailsMovie.styles';

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

const SkeletonCardDetailsMovie = styled(StyledCardMovieDetailsWrapper)`
  background-color: ${({ theme }) => theme.colors.surface};
`;

const SkeletonImageDetailsMovie = styled(SkeletonBlock)`
  flex: 1;
  height: 600px;
  border-radius: 8px 8px 0 0;
  width: 350px;
`;

const SkeletonTitleDetailsMovie = styled(SkeletonBlock)`
  height: 3rem;
  margin-bottom: 20px;
  width: 70%;
`;

const SkeletonTaglineDetailsMovie = styled(SkeletonBlock)`
  height: 1.5rem;
  margin-bottom: 20px;
  width: 50%;
`;

const SkeletonTextDetailsMovie = styled(SkeletonBlock)`
  height: 200px;
  width: 100%;
  margin-bottom: 20px;
`;

const SkeletonContentDetailsMovie = styled.div`
  display: flex;
  flex-direction: column;
  flex: 2;
`;

const SkeletonBadgeWrapperDetailsMovie = styled.div`
  display: flex;
  gap: 15px;
  margin-bottom: 20px;
`;

const SkeletonBadgeItemDetailsMovie = styled(SkeletonBlock)`
  height: 30px;
  min-width: 80px;
`;

export {
  SkeletonBlock,
  SkeletonCardDetailsMovie,
  SkeletonImageDetailsMovie,
  SkeletonTextDetailsMovie,
  SkeletonTitleDetailsMovie,
  SkeletonContentDetailsMovie,
  SkeletonTaglineDetailsMovie,
  SkeletonBadgeWrapperDetailsMovie,
  SkeletonBadgeItemDetailsMovie,
};
