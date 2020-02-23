import React from 'react'
import { Content } from 'components/Content'
import { Spacing } from 'components/Spacing'
import styled from 'styled-components'
import { Card } from 'components/Loading/Card'
import { AnimatedLine } from 'components/Loading/AnimatedLine'

const Grid = styled.div`
  display: grid;
  grid-gap: 1rem;
  grid-template-columns: 1fr 1fr 1fr;
`

export const Loading: React.FC = () => (
  <Content>
    <Spacing outer="2rem 0">
      <AnimatedLine width="16rem" height="2rem" />
    </Spacing>
    <Spacing outer="2rem 0">
      <AnimatedLine width="12rem" height="1rem" />
    </Spacing>
    <Grid>
      <Card />
      <Card />
      <Card />
    </Grid>
  </Content>
)
