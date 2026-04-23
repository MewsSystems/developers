import styled from "styled-components"

export const StyledSearch = styled.input`
  background-color: transparent;
  border: 2px solid ${({ theme }) => theme.colors.primary};
  border-radius: 50px;
  font-family: inherit;
  font-size: 1rem;
  padding: 0.5rem 1rem;
  color: #fff;

  &::placeholder {
    color: ${({ theme }) => theme.colors.tertiary};
  }

  &:focus {
    outline: none;
    background-color: ${({ theme }) => theme.colors.primary};
  }
`
