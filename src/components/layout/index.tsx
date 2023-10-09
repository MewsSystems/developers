import styled from "styled-components"

interface LayoutProps {
  children: React.ReactNode
}

export const Layout = ({ children }: LayoutProps) => {
  return <Wrapper>{children}</Wrapper>
}

const Wrapper = styled.div`
  margin: 0 auto;
  padding: 20px;
  max-width: 1200px;
`
