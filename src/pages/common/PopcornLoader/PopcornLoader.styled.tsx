import styled, {keyframes} from 'styled-components';

const bounceAndRotate = keyframes`
  0%, 100% {
    transform: translateY(0) rotate(0deg);
  }
  25% {
    transform: translateY(-15px) rotate(-5deg);
  }
  75% {
    transform: translateY(-5px) rotate(5deg);
  }
  50% {
    transform: translateY(-20px) rotate(0deg);
  }
`;

export const LoadingContainer = styled.div`
  width: 100%;
  min-height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;

  img {
    animation: ${bounceAndRotate} 0.2s ease-in-out infinite;
    transform-origin: center bottom;
  }
`;
