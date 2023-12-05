import styled from "styled-components"

const Input = styled.input`
  padding: 1em 1.5em;
  background-color: ${(props) => props.theme.white};
  color: ${(props) => props.theme.main};
  border: 1px solid ${(props) => props.theme.light_gray};
  outline: none;
  border-radius: 0.375rem;
  max-width: 500px;
  &:active,
  &:focus {
    border-color: ${(props) => props.theme.primary};
  }
`

export default Input
