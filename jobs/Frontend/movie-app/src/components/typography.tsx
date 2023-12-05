import styled from "styled-components"

const Title = styled.h1`
  padding: 0em;
  margin: 0em;
  font-size: 1.3em;
  font-weight: bold;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  font-family: ${(props) => props.theme.font.main};
`

export { Title }
