import { useSelector } from '@/lib/hooks'

type Props = {
  pageRange: number
  totalPages: number
  onSetPage: (_: number) => void
}

export const usePagination = ({ pageRange, totalPages, onSetPage }: Props) => {
  const activePage = Number(useSelector((state) => state.filter.page))

  const setPage = (page: number) => {
    if (page < 1 || page > totalPages) {
      return
    }

    onSetPage(page)
  }

  const getPages = () => {
    let startPage: number, endPage: number

    if (totalPages <= pageRange) {
      // Less than pageRange total pages, so show all pages
      startPage = 1
      endPage = totalPages
    } else {
      const maxPagesBeforeActivePage = Math.floor(pageRange / 2)
      const maxPagesAfterActivePage = Math.ceil(pageRange / 2) - 1

      if (activePage <= maxPagesBeforeActivePage) {
        // Active page near the start
        startPage = 1
        endPage = pageRange
      } else if (activePage + maxPagesAfterActivePage >= totalPages) {
        // Active page near the end
        startPage = totalPages - pageRange + 1
        endPage = totalPages
      } else {
        // Active page in the middle
        startPage = activePage - maxPagesBeforeActivePage
        endPage = activePage + maxPagesAfterActivePage
      }
    }

    return Array(endPage + 1 - startPage)
      .fill(0)
      .map((_, i) => i + startPage)
  }

  return {
    activePage,
    pages: getPages(),
    setPage: (page: number) => setPage(page),
  }
}
