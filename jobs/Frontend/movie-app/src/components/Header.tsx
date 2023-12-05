import styled from "styled-components"

const Header = styled.header`
  padding: 1em 1.5em;
  background-color: ${(props) => props.theme.white};
  box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.1);
  color: ${(props) => props.theme.primary};
  border-bottom: 1px solid ${(props) => props.theme.white};
`

export default Header
