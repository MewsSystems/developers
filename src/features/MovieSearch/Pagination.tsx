import { Button } from '@/components/ui/button'
import { ChevronLeft, ChevronRight } from 'lucide-react'

type Props = {
  page: number
  totalPages: number
  setPage: (newPage: number) => void
}

const MAX_VISIBLE_PAGES = 5

export const Pagination = ({ page, setPage, totalPages }: Props) => {
  const getVisiblePages = () => {
    const visiblePages = []
    if (totalPages <= 1) return []

    if (totalPages <= MAX_VISIBLE_PAGES) {
      for (let pageNumber = 1; pageNumber <= totalPages; pageNumber++) {
        visiblePages.push(pageNumber)
      }
      return visiblePages
    }

    visiblePages.push(1)

    if (page > 3) {
      visiblePages.push('…')
    }

    const middlePageNumbers = []
    for (
      let pageNumber = Math.max(2, page - 1);
      pageNumber <= Math.min(totalPages - 1, page + 1);
      pageNumber++
    ) {
      middlePageNumbers.push(pageNumber)
    }
    visiblePages.push(...middlePageNumbers)

    if (page < totalPages - 2) {
      visiblePages.push('…')
    }

    visiblePages.push(totalPages)

    return visiblePages
  }

  const visiblePages = getVisiblePages()

  return (
    <div className="flex justify-center items-center gap-2 mt-8 mb-8">
      <Button
        variant="outline"
        size="icon"
        onClick={() => setPage(page - 1)}
        disabled={page === 1}
        aria-label="Go to previous page"
      >
        <ChevronLeft size={18} />
      </Button>

      {visiblePages.map((pageNum, index) =>
        typeof pageNum === 'number' ? (
          <Button
            key={index}
            variant={page === pageNum ? 'default' : 'outline'}
            onClick={() => setPage(pageNum)}
          >
            {pageNum}
          </Button>
        ) : (
          <span key={index} className="px-2">
            {pageNum}
          </span>
        ),
      )}

      <Button
        variant="outline"
        size="icon"
        onClick={() => setPage(page + 1)}
        disabled={page >= totalPages}
        aria-label="Go to next page"
      >
        <ChevronRight size={18} />
      </Button>
    </div>
  )
}
