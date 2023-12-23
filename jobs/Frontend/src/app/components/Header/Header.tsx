'use client'

import { Logo, Typography } from '@/components'
import { useIsMobile } from '@/hooks'

export const Header = () => {
  const { isMobile } = useIsMobile()

  return (
    <>
      <Logo />
      {!isMobile && (
        <Typography userSelect={false} variant="primaryHeading">
          Movies
        </Typography>
      )}
    </>
  )
}
