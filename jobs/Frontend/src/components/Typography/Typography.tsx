'use client'

import { ReactNode } from 'react'
import { typographyVariants } from './Typography.styles'
import type { TypographyVariants } from './Typography.styles'

type Props = {
  variant?: TypographyVariants
  color?: 'primary' | 'secondary'
  userSelect?: boolean
  children: ReactNode
}

export const Typography = ({
  variant = 'tertiarySpan',
  color = 'primary',
  userSelect = true,
  children,
  ...props
}: Props) => {
  const Component = typographyVariants[variant]

  return (
    <Component $color={color} $userSelect={userSelect} {...props}>
      {children}
    </Component>
  )
}
