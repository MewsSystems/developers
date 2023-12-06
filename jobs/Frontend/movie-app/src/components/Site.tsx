import { ReactNode } from "react"
import styled from "styled-components"

const SiteWrapper = styled.div`
  width: 100%;
  display: flex;
  justify-content: center;
`

const SiteContainer = styled.div`
  padding: 1.5em;
  color: ${(props) => props.theme.main};
  font-family: ${(props) => props.theme.font.main};
  font-size: 1em;
  width: 1024px;
`

function Site({ children }: { children?: ReactNode }) {
  return (
    <SiteWrapper>
      <SiteContainer>{children}</SiteContainer>
    </SiteWrapper>
  )
}

export default Site
