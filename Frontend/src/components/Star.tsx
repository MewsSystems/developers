import React from 'react'
import { Icon } from './Icon'
import { SizeProp } from '@fortawesome/fontawesome-svg-core'

export interface StarProps {
  type: 'empty' | 'half' | 'full'
  color?: string
  size?: SizeProp
}

export const Star: React.FC<StarProps> = ({ type, color, size }) => {
  switch (type) {
    case 'empty':
      return <Icon icon={['far', 'star']} color={color} size={size} />
    case 'half':
      return <Icon icon="star-half-alt" color={color} size={size} />
    case 'full':
    default:
      return <Icon icon="star" color={color} size={size} />
  }
}
