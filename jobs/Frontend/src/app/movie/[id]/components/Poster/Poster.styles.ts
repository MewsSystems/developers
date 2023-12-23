import { ChevronLeftIcon } from '@heroicons/react/24/outline'
import Link from 'next/link'
import styled from 'styled-components'

export const PosterHeader = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 32px;
`

export const StyledLink = styled(Link)`
  display: flex;
  justify-content: center;
  margin-top: 2px;

  &:hover {
    opacity: 0.75;
  }
`

export const StyledBrowse = styled.span`
  display: flex;
  align-items: center;
`

export const StyledChevronLeftIcon = styled(ChevronLeftIcon)`
  width: 30px;
  height: 30px;
  padding-right: 8px;
  align-self: center;
`
