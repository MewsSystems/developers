'use client'

import { Wrapper } from './Paginator.styles'
import { PaginatorButton } from './PaginatorButton'
import {
  StyledChevronLeftIcon,
  StyledChevronRightIcon,
} from './PaginatorButton.styles'
import { usePagination } from './pagination.hook'

type Props = {
  pageRange: number
  totalPages: number
  onSetPage: (_: number) => void
}

export const Paginator = ({ pageRange, totalPages, onSetPage }: Props) => {
  const { activePage, pages, setPage } = usePagination({
    pageRange,
    totalPages,
    onSetPage,
  })

  const handlePreviousPage = () => setPage(activePage - 1)
  const handleNextPage = () => setPage(activePage + 1)

  return (
    <Wrapper aria-label="pagination">
      <PaginatorButton
        isDisabled={activePage === 1}
        onClick={handlePreviousPage}
      >
        <StyledChevronLeftIcon />
      </PaginatorButton>

      {pages.map((page) => (
        <PaginatorButton
          key={page}
          onClick={() => setPage(page)}
          isActive={activePage === page}
          {...(activePage === page && { 'aria-current': 'page' })}
          aria-label={`page ${page}`}
        >
          <span>{page}</span>
        </PaginatorButton>
      ))}

      <PaginatorButton
        isDisabled={activePage === totalPages}
        onClick={handleNextPage}
      >
        <StyledChevronRightIcon />
      </PaginatorButton>
    </Wrapper>
  )
}
