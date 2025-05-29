import styled, {keyframes} from 'styled-components';

const pulse = keyframes`
  0% {
    opacity: 1;
  }
  50% {
    opacity: 0.4;
  }
  100% {
    opacity: 1;
  }
`;

const MovieCoverSkeleton = styled.div`
  position: relative;
  width: 100%;
  height: 200px;
  background-color: #e8e8e8;
  border-radius: 4px;
  margin-bottom: 16px;
  overflow: hidden;
  animation: ${pulse} 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;

  &::after {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
    transform: translateX(-100%) skewX(-15deg);
    animation: ${pulse} 2.5s ease-in-out infinite;
  }
`;

const MovieTitle = styled(MovieCoverSkeleton)`
  width: 60%;
  height: 24px;
  margin-bottom: 8px;
`;

const MovieDescription = styled(MovieCoverSkeleton)`
  width: 100%;
  height: 80px;
`;

const MovieCard = styled.div`
  padding: 16px;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-bottom: 24px;
`;

const SkeletonGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 24px;
  padding: 24px;
`;

export const LoadingSkeleton = () => {
  return (
    <SkeletonGrid>
      {Array.from({length: 6}).map((_, index) => (
        <MovieCard key={index}>
          <MovieCoverSkeleton />
          <MovieTitle />
          <MovieDescription />
        </MovieCard>
      ))}
    </SkeletonGrid>
  );
};

LoadingSkeleton.displayName = 'LoadingSkeleton';
