import styled from "styled-components"

const BadgeContainer = styled.div`
  display: flex;
`

const Badge = styled.div`
  padding: 0.5em 1em;
  background-color: ${(props) => props.theme.lightest_gray};
  color: ${(props) => props.theme.main};
  font-size: 0.8em;
  border-radius: 1rem;
  width: max-content;
  margin-right: 10px;
`

export { Badge, BadgeContainer }
