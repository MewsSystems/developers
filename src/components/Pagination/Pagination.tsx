import type { UsePaginationReturn } from "../../hooks/usePagination"
import { Ellipsis, PageInfo, PaginationButton, PaginationContainer } from "./Pagination.styles"

export const Pagination = ({
  currentPage,
  totalPages,
  visiblePages,
  canGoPrevious,
  canGoNext,
  goToPage,
  goToPrevious,
  goToNext,
}: UsePaginationReturn) => {
  if (totalPages <= 1) return null

  const showFirstEllipsis = visiblePages[0] > 2
  const showLastEllipsis = visiblePages[visiblePages.length - 1] < totalPages - 1

  return (
    <PaginationContainer data-testid="pagination">
      <PaginationButton
        onClick={goToPrevious}
        disabled={!canGoPrevious}
        aria-label="Go to previous page"
      >
        ‹
      </PaginationButton>

      {visiblePages[0] > 1 && (
        <PaginationButton
          onClick={() => goToPage(1)}
          $isActive={currentPage === 1}
          aria-label="Go to page 1"
        >
          1
        </PaginationButton>
      )}

      {showFirstEllipsis && <Ellipsis>…</Ellipsis>}

      {visiblePages.map((page) => (
        <PaginationButton
          key={page}
          onClick={() => goToPage(page)}
          $isActive={currentPage === page}
          aria-label={`Go to page ${page}`}
          aria-current={currentPage === page ? "page" : undefined}
          data-testid={`pagination-page-${page}`}
        >
          {page}
        </PaginationButton>
      ))}

      {showLastEllipsis && <Ellipsis>…</Ellipsis>}

      {visiblePages[visiblePages.length - 1] < totalPages && (
        <PaginationButton
          onClick={() => goToPage(totalPages)}
          $isActive={currentPage === totalPages}
          aria-label={`Go to page ${totalPages}`}
        >
          {totalPages}
        </PaginationButton>
      )}

      <PaginationButton onClick={goToNext} disabled={!canGoNext} aria-label="Go to next page">
        ›
      </PaginationButton>

      <PageInfo>
        Page {currentPage} of {totalPages}
      </PageInfo>
    </PaginationContainer>
  )
}
