import styled from "styled-components"

const Input = styled.input`
  padding: 1em 1.5em;
  color: ${(props) => props.theme.main};
  border: 1px solid ${(props) => props.theme.light_gray};
  border-radius: 0.5rem;
  max-width: 500px;
  font-size: 1em;
  &:focus {
    outline: 0;
    border-color: transparent;
    box-shadow: inset 0 0 0 2px ${(props) => props.theme.primary};
  }
`

export default Input
