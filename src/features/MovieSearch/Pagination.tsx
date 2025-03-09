import { Button } from '@/components/ui/button'
import { Dispatch, SetStateAction } from 'react'

type Props = {
  page: number
  totalPages: number
  setPage: Dispatch<SetStateAction<number>>
}

export const Pagination = ({ page, setPage, totalPages }: Props) => {
  return (
    <div className="flex justify-center items-center gap-4 mt-8">
      <Button onClick={() => setPage(page - 1)} disabled={page === 1}>
        Previous
      </Button>
      <span className="text-lg font-medium">
        {page} / {totalPages}
      </span>
      <Button onClick={() => setPage(page + 1)} disabled={page >= totalPages}>
        Next
      </Button>
    </div>
  )
}
