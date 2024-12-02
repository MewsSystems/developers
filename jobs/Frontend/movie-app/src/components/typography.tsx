import styled from "styled-components"
import { Link } from "react-router-dom"

const Title = styled.h1`
  padding: 0em;
  margin: 0em;
  font-size: 1.3em;
  font-weight: bold;
  text-transform: uppercase;
  letter-spacing: 0.05em;
`

const StyledLink = styled(Link)`
  padding: 0em;
  margin: 0em;
  text-decoration: none;
  color: ${(props) => props.theme.main};
  &:hover {
    text-decoration: underline;
  }
`

export { Title, StyledLink as Link }
