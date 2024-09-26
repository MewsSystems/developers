import { ChangeEvent, useEffect, useMemo, useRef, useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'

import { keepPreviousData, useQuery } from '@tanstack/react-query'

import { debounce } from '@mui/material'

import { fetchAPI, movieDbApiKey } from '@/api/config.ts'
import { MoviesSearchApiResponse } from '@/hooks/movies/types.ts'

export const useSearchMovie = () => {
    const navigate = useNavigate()
    const location = useLocation()
    const searchRef = useRef<HTMLInputElement>(null)

    // to prevent re-rendering in useEffect
    const queryParams = useMemo(
        () => new URLSearchParams(location.search),
        [location.search]
    )

    const initialQuery = queryParams.get('query') || ''
    const initialPage = Number(queryParams.get('page')) || 1
    const [searchQuery, setSearchQuery] = useState<string>(initialQuery)
    const [currentPage, setCurrentPage] = useState<number>(initialPage)

    const shouldClear = location.state?.redirectedPage === 'homepage'

    const { data, isLoading, isError } = useQuery({
        queryKey: ['searchMovie', searchQuery, currentPage],
        queryFn: () =>
            fetchAPI(
                `search/movie?api_key=${movieDbApiKey}&query=${searchQuery}&page=${currentPage}`
            ),
        // when redirected to homepage do not keep previous data
        placeholderData: !shouldClear ? keepPreviousData : undefined,
        enabled: searchQuery.length > 2,
    })

    const { results, total_pages: totalPages } =
        (data as MoviesSearchApiResponse) || {}

    const handleSearchChange = debounce((search: string) => {
        setSearchQuery(search)
        setCurrentPage(1)
        navigate(`?query=${encodeURIComponent(search)}&page=1`)
    }, 500)

    const handlePageChange = (_: ChangeEvent<unknown>, page: number) => {
        setCurrentPage(page)
        window.scroll({
            top: 0,
            left: 0,
            behavior: 'smooth',
        })
        navigate(`?query=${encodeURIComponent(searchQuery)}&page=${page}`)
    }

    const handleClear = () => {
        if (searchRef.current) searchRef.current.value = ''
        setCurrentPage(1)
        setSearchQuery('')
    }

    useEffect(() => {
        setSearchQuery(queryParams.get('query') || '')
        setCurrentPage(Number(queryParams.get('page')) || 1)

        // to make sure TextField components is updated accordingly
        if (searchRef.current) {
            searchRef.current.value = queryParams.get('query') || ''
        }
    }, [location.search, queryParams])

    return {
        currentPage,
        handleClear,
        handlePageChange,
        handleSearchChange,
        isError,
        isLoading,
        results,
        searchQuery,
        searchRef,
        totalPages,
    }
}
