'use client'

import { Container, PaginatorWrapper } from './MovieList.styles'
import { Paginator } from '@/components'
import { useIsMobile } from '@/hooks'
import { ReactNode } from 'react'

type Props = {
  totalPages: number
  children: ReactNode
  // eslint-disable-next-line unused-imports/no-unused-vars
  onSetPage: (page: number) => void
}

export const BaseMovieList = ({ totalPages, children, onSetPage }: Props) => {
  const { isMobile } = useIsMobile()

  return (
    <>
      <Container>{children}</Container>
      <PaginatorWrapper>
        <Paginator
          onSetPage={onSetPage}
          totalPages={totalPages}
          pageRange={isMobile ? 3 : 5}
        />
      </PaginatorWrapper>
    </>
  )
}
