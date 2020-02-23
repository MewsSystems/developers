import React, { ReactNodeArray } from 'react'
import styled from 'styled-components'
import { COLORS } from 'constants/colors'

export const getFontStyle = (
  level: HeadingLevel
): { fontSize: string; margin: string } => {
  switch (level) {
    case 1:
      return { fontSize: '2rem', margin: '1.3rem 0' }
    case 2:
      return { fontSize: '1.75rem', margin: '1rem 0' }
    case 3:
      return { fontSize: '1.3rem', margin: '0.75rem 0' }
    case 4:
      return { fontSize: '1.1rem', margin: '0.75rem 0' }
    default:
      return { fontSize: '0.9rem', margin: '0.6rem 0' }
  }
}

export interface StyledHeadingProps {
  fontSize: string
  margin: string
  color: string
}

export const StyledHeading = styled.div<StyledHeadingProps>`
  margin: ${({ margin }) => margin};
  color: ${({ color }) => color};
  font-size: ${({ fontSize }) => fontSize};
`
export type HeadingLevel = 1 | 2 | 3 | 4 | 5
export type HeadingTags = 'h1' | 'h2' | 'h3' | 'h4' | 'h5'

export interface HeadingProps {
  level: HeadingLevel
  color?: string
  children?: React.ReactNode | ReactNodeArray
  className?: string
}

export const Heading: React.FC<HeadingProps> = ({
  level,
  color = COLORS.BLACK,
  children,
  className,
}) => {
  const { fontSize, margin } = getFontStyle(level)
  const as = `h${level}` as HeadingTags

  return (
    <StyledHeading
      fontSize={fontSize}
      margin={margin}
      color={color}
      className={className}
      as={as}
    >
      {children}
    </StyledHeading>
  )
}
