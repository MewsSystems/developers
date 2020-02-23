import React from 'react'
import styled from 'styled-components'
import { COLORS } from 'constants/colors'
import { Content } from 'components/Content'
import { AnimatedLine } from './AnimatedLine'
import { Spacing } from 'components/Spacing'

const Header = styled.div`
  height: 10.6rem;
  background: ${COLORS.GRAY};
`

export const App: React.FC = () => (
  <>
    <Header />
    <Content>
      <Spacing outer="2rem 0">
        <AnimatedLine width="20rem" height="2rem" />
      </Spacing>
    </Content>
  </>
)
