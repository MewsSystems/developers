'use client'

import { CanvasProps } from '@react-three/fiber'

import { StyledCanvas } from './Canvas.styles'

export const Canvas = ({ children, ...props }: CanvasProps) => {
  return <StyledCanvas {...props}>{children}</StyledCanvas>
}
