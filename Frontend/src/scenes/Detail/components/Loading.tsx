import React from 'react'
import { StyledHeroBanner } from 'components/HeroBanner'
import { Content } from 'components/Content'
import { AnimatedLine } from 'components/Loading/AnimatedLine'

export const Loading: React.FC = () => (
  <div>
    <StyledHeroBanner />
    <Content>
      <AnimatedLine height="2rem" width="80%" />
    </Content>
  </div>
)
