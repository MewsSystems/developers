import styled from "styled-components"

export const StyledButton = styled.button`
  background-color: transparent;
  border-radius: 50px;
  border: 2px solid ${({ theme }) => theme.colors.primary};
  color: #fff;
  cursor: pointer;
  font-family: inherit;
  font-size: 1.5rem;
  margin: 30px 0;
  padding: 0.5rem 1rem;
  padding: 15px 30px;

  &:hover {
    background-color: ${({ theme }) => theme.colors.secondary};
    color: white;
  }

  &:disabled {
    cursor: not-allowed;
    opacity: 0.4;
  }
`
