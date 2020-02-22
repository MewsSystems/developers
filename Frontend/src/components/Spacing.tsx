import React from 'react'
import styled from 'styled-components'

export const StyledSpacing = styled.div<SpacingProps>`
  margin: ${({ outer }) => outer};
  padding: ${({ inner }) => inner};
`

export interface SpacingProps {
  inner?: string
  outer?: string
  className?: string
  children?: React.ReactNode | React.ReactNodeArray
}

export const Spacing: React.FC<SpacingProps> = ({
  inner,
  outer,
  className,
  children,
}) => (
  <StyledSpacing outer={outer} inner={inner} className={className}>
    {children}
  </StyledSpacing>
)
