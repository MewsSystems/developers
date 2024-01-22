import styled from "styled-components"

export const StyledButton = styled.button`
  background-color: transparent;
  border: 2px solid ${({ theme }) => theme.colors.primary};
  border-radius: 50px;
  font-family: inherit;
  font-size: 1.5rem;
  padding: 0.5rem 1rem;
  color: #fff;
  margin: 30px 0;
  padding: 15px 30px;
`
