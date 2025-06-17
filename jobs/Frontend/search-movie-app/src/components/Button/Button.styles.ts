import styled from 'styled-components';

export const StyledButton = styled.button`
  background-color: ${({ theme }) => theme.colors.primary};
  border-radius: 24px;
  border-style: none;
  box-shadow:
    rgba(0, 0, 0, 0.2) 0 3px 5px -1px,
    rgba(0, 0, 0, 0.14) 0 6px 10px 0,
    rgba(0, 0, 0, 0.12) 0 1px 18px 0;
  color: ${({ theme }) => theme.colors.onPrimary};
  cursor: pointer;
  height: 48px;
  justify-content: center;
  padding: 2px 24px;
  text-align: center;
  transition:
    box-shadow 280ms cubic-bezier(0.4, 0, 0.2, 1),
    opacity 15ms linear 30ms,
    background-color 200ms ease-in-out,
    transform 270ms cubic-bezier(0, 0, 0.2, 1) 0ms;
  &:focus {
    outline: none;
    border: 2px solid ${({ theme }) => theme.colors.hoverPrimary};
  }
  &:hover {
    background-color: ${({ theme }) => theme.colors.hoverPrimary};
    color: ${({ theme }) => theme.colors.onPrimary};
  }
`;
