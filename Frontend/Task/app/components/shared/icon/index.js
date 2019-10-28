import React from 'react'
import styled from 'styled-components'
import { number, oneOf, string } from 'prop-types'

import { trend } from '../../../constants'

import Growth from './icons/growth'
import Decline from './icons/decline'
import Stagnation from './icons/stagnation'

const icons = {
  [trend.DECLINING]: Decline,
  [trend.GROWING]: Growth,
  [trend.STAGNATING]: Stagnation,
}

const renderIcon = (icon, color) => {
  const SelectedIcon = icons[icon]

  return <SelectedIcon color={color} />
}

const IconWrapper = styled.div`
  width: ${({ size }) => size}px;
  height: ${({ size }) => size}px;
  margin: 0;
`

const Icon = ({ className, color, icon, size }) => (
  <IconWrapper className={className} size={size}>
    {renderIcon(icon, color)}
  </IconWrapper>
)

Icon.propTypes = {
  className: string,
  color: string.isRequired,
  icon: oneOf(Object.keys(icons)).isRequired,
  size: number.isRequired,
}

export default Icon
