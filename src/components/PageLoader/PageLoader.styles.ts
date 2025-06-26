import styled, { keyframes } from "styled-components"

const fadeIn = keyframes`
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
`

const pulse = keyframes`
  0%, 100% {
    transform: scale(1);
    opacity: 0.7;
  }
  50% {
    transform: scale(1.1);
    opacity: 1;
  }
`

export const Container = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100vh;
  gap: 1rem;
  animation: ${fadeIn} 0.3s ease-in-out;
  background-color: ${({ theme }) => theme.colors.background};
`

export const Spinner = styled.div`
  color: ${({ theme }) => theme.colors.primary};
  animation: ${pulse} 1.5s ease-in-out infinite;
`
