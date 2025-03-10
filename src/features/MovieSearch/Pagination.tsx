import { Button } from '@/components/ui/button'
import { ChevronLeft, ChevronRight } from 'lucide-react'

type Props = {
  page: number
  totalPages: number
  setPage: (newPage: number) => void
}

export const Pagination = ({ page, setPage, totalPages }: Props) => {
  const getVisiblePages = () => {
    const pages = []
    if (totalPages <= 1) return []

    if (totalPages <= 5) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i)
      }
      return pages
    }

    pages.push(1)

    if (page > 3) {
      pages.push('...')
    }

    const middlePages = []
    for (
      let i = Math.max(2, page - 1);
      i <= Math.min(totalPages - 1, page + 1);
      i++
    ) {
      middlePages.push(i)
    }
    pages.push(...middlePages)

    if (page < totalPages - 2) {
      pages.push('...')
    }

    pages.push(totalPages)

    return pages
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
            ...
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
