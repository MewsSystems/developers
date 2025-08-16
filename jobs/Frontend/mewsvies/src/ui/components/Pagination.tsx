// @ts-ignore
import { generate } from '@bramus/pagination-sequence'
import styles from './Pagination.module.scss'

interface PaginationProps {
    page: number
    totalPages: number
    onPageChange: (page: number) => void
}

export default function Pagination({
    page,
    totalPages,
    onPageChange,
}: PaginationProps) {
    const handlePageChange = (newPage: number) => {
        if (newPage < 1 || newPage > totalPages) {
            return
        }
        onPageChange(newPage)
    }

    const pageNumbersButton = generate(page, totalPages, 1).map(
        (pageNumber: string | number, index: number) => {
            const number = `${pageNumber}`
            const isDisabled = number === '…'

            return (
                <button
                    key={index}
                    disabled={isDisabled}
                    onClick={() => handlePageChange(parseInt(number, 10))}
                >
                    {pageNumber}
                </button>
            )
        }
    )

    return (
        <div className={styles.pagination}>
            <button onClick={() => handlePageChange(page - 1)}>{'<'}</button>
            {pageNumbersButton}
            <button onClick={() => handlePageChange(page + 1)}>{'>'}</button>
        </div>
    )
}
