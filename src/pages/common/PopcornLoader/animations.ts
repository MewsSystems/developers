import {keyframes} from 'styled-components';

export const bounceAndRotate = keyframes`
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
