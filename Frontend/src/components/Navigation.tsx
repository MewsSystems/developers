import { COLORS } from 'constants/colors'
import React from 'react'
import { Link } from '@reach/router'
import styled from 'styled-components'

const Container = styled.div`
  display: grid;
  grid-auto-columns: min-content;
  grid-auto-flow: column;
  grid-gap: 1rem;
`

const StyledLink = styled(Link)`
  color: ${COLORS.WHITE};
  text-decoration: none;

  @media (hover: hover) {
    :hover {
      color: ${COLORS.GRAY};
    }
  }
`

export interface NavigationItemProps {
  title: React.ReactNode | React.ReactNodeArray
  to: string
  key: string
}

export interface NavigationProps {
  items: NavigationItemProps[]
  className?: string
}

export const Navigation: React.FC<NavigationProps> = ({ items, className }) => (
  <Container className={className}>
    {items.map(({ title, to, key }) => (
      <StyledLink to={to} key={key}>
        {title}
      </StyledLink>
    ))}
  </Container>
)
