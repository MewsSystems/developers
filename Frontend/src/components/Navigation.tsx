import { COLORS } from 'constants/colors'
import React, { useCallback } from 'react'
import { Link } from '@reach/router'
import styled from 'styled-components'
import { useTrackEvent } from 'hooks/useTrackEvent'
import { EVENT_CATEGORY, EVENT_ACTION } from 'constants/tracking'

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

export const Navigation: React.FC<NavigationProps> = ({ items, className }) => {
  const { setTrackEvent } = useTrackEvent()

  const handleItemClick = useCallback(
    (title: string) => {
      setTrackEvent({
        eventCategory: EVENT_CATEGORY.MAIN_NAVIGATION,
        eventAction: EVENT_ACTION.MAIN_NAVIGATION.CLICK_ITEM,
        eventLabel: title,
      })
    },
    [setTrackEvent]
  )

  return (
    <Container className={className}>
      {items.map(({ title, to, key }) => (
        <StyledLink
          to={to}
          key={key}
          onClick={() => handleItemClick(String(title))}
        >
          {title}
        </StyledLink>
      ))}
    </Container>
  )
}
