import { Spacing } from 'components/Spacing'
import { Header } from './components/Header'
import { Footer } from './components/Footer'
import React from 'react'
import styled from 'styled-components'

const Container = styled.div`
  min-height: calc(100vh - 54px);
`

export interface LayoutProps {
  children: React.ReactNode | React.ReactNodeArray
}

export const Layout: React.FC<LayoutProps> = ({ children }) => (
  <div>
    <Container>
      <Header />
      <Spacing outer="0 0 54px;">{children}</Spacing>
    </Container>
    <Footer />
  </div>
)
