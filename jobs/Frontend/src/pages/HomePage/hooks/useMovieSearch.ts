import { useRef, useState } from 'react'
import { debounce } from 'lodash'
import { useMovieSearchQuery } from '../../../api/movies/hooks'
import { MovieSearch } from '../types'

export const useMovieSearch = (): MovieSearch => {
    const [currentSearchTitle, setCurrentSearchTitle] = useState('')
    const [currentPage, setCurrentPage] = useState(1)

    const {
        data: searchData,
        isLoading,
        isError,
    } = useMovieSearchQuery({
        movieTitle: currentSearchTitle,
        page: currentPage,
    })

    const searchInputRef = useRef<HTMLInputElement>(null)

    const submitSearchedTitle = debounce((movieTitle: string) => {
        setCurrentSearchTitle(movieTitle)
    }, 500)

    const handleChangePage = (
        event: React.ChangeEvent<unknown>,
        value: number,
    ) => {
        setCurrentPage(value)
        window.scrollTo({ top: 0, left: 0, behavior: 'smooth' })
    }

    const clearTitle = () => {
        setCurrentSearchTitle('')

        if (searchInputRef.current) {
            searchInputRef.current.value = ''
        }
    }

    return {
        currentSearchTitle,
        clearTitle,
        searchInputRef,
        submitSearchedTitle,
        searchData,
        isLoading,
        isError,
        handleChangePage,
    }
}
