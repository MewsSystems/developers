import { COLORS } from 'constants/colors'
import { IconName } from 'constants/icons'
import { SizeProp } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React from 'react'
import styled from 'styled-components'

export const Container = styled.div<Pick<IconProps, 'hoverColor'>>`
  svg {
    vertical-align: initial;

    @media (hover: hover) {
      :hover {
        color: ${({ hoverColor }) => hoverColor};
      }
    }
  }
`

export interface IconProps {
  color?: string
  hoverColor?: string
  icon: IconName
  size?: SizeProp
  onClick?: (e: React.MouseEvent<HTMLImageElement>) => void
  className?: string
}

export const Icon: React.FC<IconProps> = ({
  color = COLORS.BLACK,
  hoverColor,
  icon,
  size = '1x',
  onClick,
  className,
}) => (
  <Container onClick={onClick} hoverColor={hoverColor} className={className}>
    <FontAwesomeIcon color={color} size={size} icon={icon} />
  </Container>
)
