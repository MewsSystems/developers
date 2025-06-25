import { useState } from "react"

export interface UsePaginationProps {
  totalPages: number
  initialPage?: number
  currentPage?: number
  maxVisiblePages?: number
  onPageChange?: (page: number) => void
}

export interface UsePaginationReturn {
  currentPage: number
  totalPages: number
  visiblePages: number[]
  canGoPrevious: boolean
  canGoNext: boolean
  goToPage: (page: number) => void
  goToPrevious: () => void
  goToNext: () => void
}

export const usePagination = ({
  totalPages,
  initialPage = 1,
  currentPage: controlledCurrentPage,
  maxVisiblePages = 3,
  onPageChange,
}: UsePaginationProps): UsePaginationReturn => {
  const [internalCurrentPage, setInternalCurrentPage] = useState(initialPage)

  const isControlled = controlledCurrentPage !== undefined
  const currentPage = isControlled ? controlledCurrentPage : internalCurrentPage

  const goToPage = (page: number) => {
    if (page >= 1 && page <= totalPages) {
      if (isControlled) {
        onPageChange?.(page)
      } else {
        setInternalCurrentPage(page)
      }
    }
  }

  const goToPrevious = () => {
    goToPage(currentPage - 1)
  }

  const goToNext = () => {
    goToPage(currentPage + 1)
  }

  const canGoPrevious = currentPage > 1
  const canGoNext = currentPage < totalPages

  const getVisiblePages = () => {
    if (totalPages <= maxVisiblePages) {
      return Array.from({ length: totalPages }, (_, i) => i + 1)
    }

    const half = Math.floor(maxVisiblePages / 2)
    let start = Math.max(1, currentPage - half)
    const end = Math.min(totalPages, start + maxVisiblePages - 1)

    if (end - start + 1 < maxVisiblePages) {
      start = Math.max(1, end - maxVisiblePages + 1)
    }

    return Array.from({ length: end - start + 1 }, (_, i) => start + i)
  }

  const visiblePages = getVisiblePages()

  return {
    currentPage,
    totalPages,
    visiblePages,
    canGoPrevious,
    canGoNext,
    goToPage,
    goToPrevious,
    goToNext,
  }
}
