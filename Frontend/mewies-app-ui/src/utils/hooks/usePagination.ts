import { useEffect, useState } from 'react'

interface PaginationSettings<K> {
    data: K[]
    recordsPerPage: number
}

export const usePagination = <K>(options: PaginationSettings<K>) => {
    const [paginatedData, setData] = useState<K[][]>([])
    const [currentPage, setPage] = useState(0)

    const isFirstPage = currentPage === 0
    const isLastPage = currentPage === paginatedData.length - 1

    const paginateData = (data: K[]) => {
        let pagination: K[][] = [],
            start = 0,
            limit = data.length

        while (start < limit) {
            pagination.push(
                data.slice(start, (start += options.recordsPerPage))
            )
        }

        setData(pagination)
    }

    const onPaginateNext = () => {
        !isLastPage && setPage(currentPage + 1)
    }

    const onPaginatePrev = () => {
        !isFirstPage && setPage(currentPage - 1)
    }

    useEffect(() => {
        paginateData(options.data)
    }, [options.data])

    return {
        paginatedData: paginatedData[currentPage]
            ? paginatedData[currentPage]
            : [],
        isFirstPage,
        isLastPage,
        isDataToPaginate: paginatedData.length > 0,
        onPaginateNext,
        onPaginatePrev,
    }
}
