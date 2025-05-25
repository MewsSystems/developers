import styled, { keyframes } from 'styled-components';

const shimmer = keyframes`
  0% {
    background-position: -200% 0;
  }
  100% {
    background-position: 200% 0;
  }
`;

const SkeletonBase = styled.div`
  background: linear-gradient(90deg, 
    rgba(255, 255, 255, 0.1) 25%, 
    rgba(255, 255, 255, 0.2) 50%, 
    rgba(255, 255, 255, 0.1) 75%
  );
  background-size: 200% 100%;
  animation: ${shimmer} 1.5s infinite;
  border-radius: 8px;
`;

const Container = styled.div`
  min-height: 100vh;
  background: linear-gradient(to bottom, #1a1a1a, #2d2d2d);
  padding: 2rem;
`;

const BackButton = styled(SkeletonBase)`
  width: 120px;
  height: 24px;
  margin-bottom: 2rem;
`;

const MovieGrid = styled.div`
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: 2rem;
  background: rgba(0, 0, 0, 0.5);
  padding: 2rem;
  border-radius: 12px;
  backdrop-filter: blur(10px);
`;

const Poster = styled(SkeletonBase)`
  width: 100%;
  padding-top: 150%;
  border-radius: 8px;
`;

const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
`;

const Title = styled(SkeletonBase)`
  width: 80%;
  height: 40px;
`;

const ReleaseDate = styled(SkeletonBase)`
  width: 200px;
  height: 24px;
`;

const Rating = styled(SkeletonBase)`
  width: 150px;
  height: 24px;
`;

const Overview = styled(SkeletonBase)`
  width: 100%;
  height: 120px;
`;

export const MovieDetailSkeleton = () => {
  return (
    <Container>
      <BackButton />
      <MovieGrid>
        <Poster />
        <MovieInfo>
          <Title />
          <ReleaseDate />
          <Rating />
          <Overview />
        </MovieInfo>
      </MovieGrid>
    </Container>
  );
}; 