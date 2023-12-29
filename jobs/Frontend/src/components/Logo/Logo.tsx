'use client'

import { StyledLogo } from './Logo.styles'

type Props = {
  isColorless?: boolean
}

export const Logo = ({ isColorless = false }: Props) => (
  <StyledLogo
    $isColorless={isColorless}
    priority
    src="/logo.svg"
    width={200}
    height={26}
    alt="TMDB logo"
  />
)
