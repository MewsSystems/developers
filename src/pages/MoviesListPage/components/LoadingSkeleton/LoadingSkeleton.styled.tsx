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

export const MovieCoverSkeleton = styled.div`
  position: relative;
  width: 100%;
  aspect-ratio: 2/3;
  background-color: #e8e8e8;
  border-radius: 8px 8px 0 0;
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

export const MovieTitle = styled.div`
  width: 80%;
  height: 20px;
  margin: 12px 12px 8px;
  background-color: #e8e8e8;
  border-radius: 4px;
  animation: ${pulse} 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;
`;

export const MovieDescription = styled.div`
  width: 90%;
  height: 32px;
  margin: 0 12px 12px;
  background-color: #e8e8e8;
  border-radius: 4px;
  animation: ${pulse} 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;
`;

export const MovieCard = styled.div`
  background: white;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  height: 100%;
  display: flex;
  flex-direction: column;
`;

export const SkeletonGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 16px;
  margin: 24px 0;
  width: 100%;

  @media (max-width: 1400px) {
    grid-template-columns: repeat(4, 1fr);
  }

  @media (max-width: 1100px) {
    grid-template-columns: repeat(3, 1fr);
  }

  @media (max-width: 800px) {
    grid-template-columns: repeat(2, 1fr);
  }

  @media (max-width: 500px) {
    grid-template-columns: 1fr;
  }
`;
