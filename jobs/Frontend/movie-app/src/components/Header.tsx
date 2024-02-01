import styled from "styled-components"

const Header = styled.header`
  padding: 1em 1.5em;
  background-color: ${(props) => props.theme.white};
  box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.1);
  border-bottom: 1px solid ${(props) => props.theme.white};
  a {
    color: ${(props) => props.theme.primary};
    text-decoration: none;
  }
`

export default Header
