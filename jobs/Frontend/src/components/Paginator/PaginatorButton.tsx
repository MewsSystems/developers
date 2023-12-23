'use client'

import { MouseEventHandler, ReactElement } from 'react'
import { Typography } from '../Typography'
import { StyledButton } from './PaginatorButton.styles'

type Props = {
  isActive?: boolean
  isDisabled?: boolean
  onClick: MouseEventHandler<HTMLButtonElement>
  children: ReactElement
}

export const PaginatorButton = ({
  isActive = false,
  isDisabled = false,
  onClick,
  children,
}: Props) => {
  return (
    <StyledButton
      data-testid="paginator-button"
      $isActive={isActive}
      disabled={isDisabled}
      onClick={onClick}
    >
      <Typography userSelect={false} variant="tertiarySpan">
        {children}
      </Typography>
    </StyledButton>
  )
}
