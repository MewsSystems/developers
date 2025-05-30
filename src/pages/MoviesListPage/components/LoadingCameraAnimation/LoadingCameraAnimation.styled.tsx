import styled, {keyframes} from 'styled-components';

const rotateReel = keyframes`
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`;

const fadeInOut = keyframes`
  0%, 100% {
    opacity: 0.6;
  }
  50% {
    opacity: 1;
  }
`;

export const LoadingContainer = styled.div`
  width: 100%;
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
`;

export const CameraBody = styled.svg`
  width: 160px;
  height: 160px;
  animation: ${fadeInOut} 3s ease-in-out infinite;
`;

export const FilmTape = styled.g`
  transform-origin: center;
  animation: ${rotateReel} 4s linear infinite;
`;

export const SmallReel = styled.g`
  transform-origin: center;
  animation: ${rotateReel} 4s linear infinite;
`;
