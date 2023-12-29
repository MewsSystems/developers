import styled, { css } from 'styled-components'
import { ChevronLeftIcon, ChevronRightIcon } from '@heroicons/react/24/outline'
import { theme } from '@/theme'

type StyledTypes = { $isActive: boolean }

export const StyledButton = styled.button<StyledTypes>`
  display: flex;
  justify-content: center;
  align-items: center;
  width: 48px;
  height: 48px;
  border-radius: 2px;
  transition: background-color 0.1s;

  &:hover {
    ${({ $isActive }) =>
      !$isActive &&
      css`
        background-color: ${theme.colors.background.secondary};
        transition: background-color 0.2s;
      `}
  }

  &[disabled] {
    pointer-events: none;
    filter: opacity(0.2);
  }

  ${({ $isActive }) =>
    $isActive &&
    css`
      border: 2px solid ${theme.colors.border.primary};
    `}
`

const ChevronStyles = css`
  width: 32px;
  height: 32px;
`

export const StyledChevronLeftIcon = styled(ChevronLeftIcon)`
  ${ChevronStyles}
  transform: translateX(-1px);
`

export const StyledChevronRightIcon = styled(ChevronRightIcon)`
  ${ChevronStyles}
  transform: translateX(2px);
`
