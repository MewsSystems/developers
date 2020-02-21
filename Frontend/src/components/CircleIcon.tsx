import { COLORS } from 'constants/colors'
import { IconName } from 'constants/icons'
import { Icon } from 'components/Icon'
import React from 'react'
import styled, { css } from 'styled-components'

export const Container = styled.div<Pick<CircleIconProps, 'size' | 'onClick'>>`
  display: flex;
  align-items: center;
  justify-content: center;
  width: ${({ size }) => (size === 'small' ? '1.25rem' : '2rem')};
  height: ${({ size }) => (size === 'small' ? '1.25rem' : '2rem')};
  background: ${COLORS.GRAY};
  border-radius: 50%;

  ${({ onClick }) =>
    !!onClick &&
    css`
      cursor: pointer;

      @media (hover: hover) {
        :hover {
          border: initial;
          background: ${COLORS.LIGHT_BLUE};
        }
      }
    `}
`

export interface CircleIconProps {
  icon: IconName
  size?: 'small' | 'large'
  onClick?: (event: React.MouseEvent<HTMLDivElement>) => void
  className?: string
}

export const CircleIcon: React.FC<CircleIconProps> = ({
  icon,
  size = 'small',
  onClick,
  className,
}) => (
  <Container onClick={onClick} size={size} className={className}>
    <Icon
      icon={icon}
      color={COLORS.WHITE}
      size={size === 'small' ? 'xs' : 'sm'}
    />
  </Container>
)
