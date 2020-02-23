import React from 'react'
import { AnimatedLine } from './AnimatedLine'
import { StyledCard } from 'components/Card'
import { Spacing } from 'components/Spacing'

export const Card: React.FC = () => (
  <StyledCard>
    <Spacing outer="0 0 1rem">
      <AnimatedLine width="100%" height="2rem" />
    </Spacing>
    <AnimatedLine width="100%" height="1rem" />
  </StyledCard>
)
