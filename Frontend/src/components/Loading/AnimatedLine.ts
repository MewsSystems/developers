import styled from 'styled-components'

export interface AnimatedLineProps {
  width: string
  height: string
}

export const AnimatedLine = styled.div<AnimatedLineProps>`
  width: ${({ width }) => width};
  height: ${({ height }) => height};
  background: linear-gradient(
    90deg,
    rgba(195, 195, 195, 1) 0%,
    rgba(246, 246, 246, 1) 100%
  );
  animation: AnimationName 5s ease infinite;

  @keyframes AnimationName {
    0% {
      background-position: 0% 50%;
    }
    50% {
      background-position: 100% 50%;
    }
    100% {
      background-position: 0% 50%;
    }
  }
`
