import React from 'react'
import styled, { css } from 'styled-components'
import { BORDER_RADIUS, BOX_SHADOW } from 'constants/index'
import { COLORS } from 'constants/colors'
import { darken } from 'polished'

export const StyledButton = styled.button<
  Pick<ButtonProps, 'disabled' | 'variant' | 'fullWidth'>
>`
  width: ${({ fullWidth }) => fullWidth && '100%'};
  min-width: 12rem;
  padding: 1rem;
  text-align: center;
  text-transform: uppercase;
  border: 0;
  border-radius: ${BORDER_RADIUS.MEDIUM};
  box-shadow: ${BOX_SHADOW.MEDIUM};

  ${({ disabled, variant }) =>
    disabled
      ? css`
          background: ${COLORS.GRAY};
          color: ${COLORS.DARK_GRAY};
        `
      : css`
          background: ${variant === 'primary' ? COLORS.BLUE : COLORS.RED};
          color: ${COLORS.WHITE};
          cursor: pointer;
          transition: background ease-in 0.5s;

          :hover {
            background: ${variant === 'primary'
              ? darken(0.1, COLORS.BLUE)
              : darken(0.1, COLORS.RED)};
            transition: background ease-in 0.5s;
          }
        `}
`

export interface ButtonProps {
  variant?: 'primary' | 'secondary'
  disabled?: boolean
  fullWidth?: boolean
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void
  children?: React.ReactNode | React.ReactNodeArray
}

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  disabled,
  fullWidth,
  onClick,
  children,
}) => (
  <StyledButton
    variant={variant}
    disabled={disabled}
    fullWidth={fullWidth}
    onClick={onClick}
  >
    {children}
  </StyledButton>
)
