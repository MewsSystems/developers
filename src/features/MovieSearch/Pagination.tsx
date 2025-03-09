import { Button } from '@/components/ui/button'

type Props = {
  page: number
  totalPages: number
  setPage: (newPage: number) => void
}

export const Pagination = ({ page, setPage, totalPages }: Props) => {
  return (
    <div className="flex justify-center items-center gap-4 mt-8">
      <Button
        className="cursor-pointer"
        onClick={() => setPage(page - 1)}
        disabled={page === 1}
      >
        Previous
      </Button>
      <span className="text-lg font-medium">
        {page} / {totalPages}
      </span>
      <Button
        className="cursor-pointer"
        onClick={() => setPage(page + 1)}
        disabled={page >= totalPages}
      >
        Next
      </Button>
    </div>
  )
}
