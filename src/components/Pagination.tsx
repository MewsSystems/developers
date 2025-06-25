import styled from "styled-components"
import type { UsePaginationReturn } from "../hooks/usePagination"

const PaginationContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  gap: ${({ theme }) => theme.spacing.sm};
  margin: ${({ theme }) => theme.spacing.xl} 0;
  flex-wrap: wrap;

  @media (max-width: ${({ theme }) => theme.breakpoints.md}) {
    gap: ${({ theme }) => theme.spacing.xs};
  }
`

const PaginationButton = styled.button<{ $isActive?: boolean }>`
  min-width: 40px;
  height: 40px;
  border: 1px solid ${({ theme }) => theme.colors.border};
  background: ${({ theme, $isActive }) =>
    $isActive ? theme.colors.primary : theme.colors.background};
  color: ${({ theme, $isActive }) => ($isActive ? theme.colors.background : theme.colors.text)};
  border-radius: ${({ theme }) => theme.borderRadius.base};
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: ${({ $isActive }) => ($isActive ? "600" : "400")};
  font-size: ${({ theme }) => theme.fontSizes.sm};
  transition: all 0.2s ease;

  &:disabled {
    color: ${({ theme }) => theme.colors.textSecondary};
    cursor: not-allowed;
    opacity: 0.5;
  }

    &:hover:not(:disabled) {
    background: ${({ theme, $isActive }) =>
      $isActive ? theme.colors.primary : theme.colors.surface};
    border-color: ${({ theme, $isActive }) =>
      $isActive ? theme.colors.primary : theme.colors.primary};
    transform: translateY(-1px);
  }

  &:active:not(:disabled) {
    transform: translateY(0);
  }

  @media (max-width: ${({ theme }) => theme.breakpoints.md}) {
    min-width: 32px;
    height: 32px;
    font-size: ${({ theme }) => theme.fontSizes.xs};
  }
`

const PageInfo = styled.span`
  margin: 0 ${({ theme }) => theme.spacing.md};
  font-size: ${({ theme }) => theme.fontSizes.sm};
  color: ${({ theme }) => theme.colors.textSecondary};
  white-space: nowrap;

  @media (max-width: ${({ theme }) => theme.breakpoints.md}) {
    font-size: ${({ theme }) => theme.fontSizes.xs};
    margin: 0 ${({ theme }) => theme.spacing.sm};
  }
`

const Ellipsis = styled.span`
  padding: 0 ${({ theme }) => theme.spacing.xs};
  color: ${({ theme }) => theme.colors.textSecondary};
  user-select: none;
`

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
    <PaginationContainer>
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
