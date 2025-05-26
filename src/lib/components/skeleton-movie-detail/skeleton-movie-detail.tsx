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
  background: linear-gradient(to bottom, #2d2d2d, #3d3d3d);
  padding: 1rem;

  @media (min-width: 768px) {
    padding: 2rem;
  }
`;

const BackButton = styled(SkeletonBase)`
  width: 120px;
  height: 24px;
  margin-bottom: 1rem;

  @media (min-width: 768px) {
    margin-bottom: 2rem;
  }
`;

const MovieGrid = styled.div`
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
  background: rgba(40, 40, 40, 0.7);
  padding: 1.5rem;
  border-radius: 12px;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);

  @media (min-width: 768px) {
    grid-template-columns: 300px 1fr;
    gap: 2rem;
    padding: 2rem;
  }
`;

const Poster = styled(SkeletonBase)`
  width: 100%;
  max-width: 300px;
  margin: 0 auto;
  padding-top: 150%;
  border-radius: 8px;

  @media (min-width: 768px) {
    margin: 0;
  }
`;

const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;

  @media (min-width: 768px) {
    gap: 1.5rem;
  }
`;

const Title = styled(SkeletonBase)`
  width: 80%;
  height: 32px;
  margin: 0 auto;

  @media (min-width: 768px) {
    height: 40px;
    margin: 0;
  }
`;

const MovieDetails = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  justify-content: center;
  margin-bottom: 0.5rem;

  @media (min-width: 768px) {
    justify-content: flex-start;
  }
`;

const DetailItem = styled(SkeletonBase)`
  width: 120px;
  height: 32px;
  border-radius: 20px;

  @media (min-width: 768px) {
    height: 36px;
  }
`;

const ReleaseDate = styled(SkeletonBase)`
  width: 200px;
  height: 20px;
  margin: 0 auto;

  @media (min-width: 768px) {
    height: 24px;
    margin: 0;
  }
`;

const Rating = styled(SkeletonBase)`
  width: 150px;
  height: 20px;
  margin: 0 auto;

  @media (min-width: 768px) {
    height: 24px;
    margin: 0;
  }
`;

const Overview = styled(SkeletonBase)`
  width: 100%;
  height: 100px;
  margin: 0 auto;

  @media (min-width: 768px) {
    height: 120px;
    margin: 0;
  }
`;

export const MovieDetailSkeleton = () => {
  return (
    <Container>
      <BackButton />
      <MovieGrid>
        <Poster />
        <MovieInfo>
          <Title />
          <MovieDetails>
            <DetailItem />
            <DetailItem />
            <DetailItem />
          </MovieDetails>
          <ReleaseDate />
          <Rating />
          <Overview />
        </MovieInfo>
      </MovieGrid>
    </Container>
  );
}; 