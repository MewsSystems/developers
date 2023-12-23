'use client'

import { StyledLayout } from './layout.styles'

export default function MovieDetailLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return <StyledLayout>{children}</StyledLayout>
}
