import { useEffect, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import { debounce } from '../utils/debounce'
import Search from '../components/Search.tsx'
import { getMovieSearch } from '../../domain/services/movie.service.ts'
import { MovieSearchResult } from '../../domain/interfaces/movie.interface.ts'
import MovieList from '../components/MovieList.tsx'
import Pagination from '../components/Pagination.tsx'
import { getInitialSearchResult } from '../utils/getInitialSearchResult.ts'

function Home() {
    const [searchParams, setSearchParams] = useSearchParams()

    const [searchResult, setSearchResult] = useState<MovieSearchResult>(
        getInitialSearchResult()
    )
    const [query, setQuery] = useState<string>(searchParams.get('query') || '')
    const [page, setPage] = useState<number>(
        parseInt(searchParams.get('page') || '1', 10)
    )
    const [hasError, setHasError] = useState<boolean>(false)

    const showPagination: boolean = searchResult?.total_pages > 1 || false

    const searchMovie = async () => {
        if (!query) {
            return
        }

        if (hasError) {
            setHasError(false)
        }

        try {
            // TODO: add loading component
            const list = await getMovieSearch({ query, page })
            setSearchResult(list)
        } catch (error) {
            setHasError(true)
        }
    }

    const setQueryParams = () => {
        setSearchParams({ query, page: `${page}` })
    }

    /*
     * useEffect to fetch the movie list when the query or page changes
     * The list is also fetched when you land on the page and there are some query params
     */
    useEffect(() => {
        setQueryParams()
        searchMovie()
    }, [query, page])

    return (
        <>
            <Search query={query} onSearch={debounce(setQuery, 500)} />
            {hasError && <div>Something went wrong...</div>}
            <MovieList movies={searchResult.results || []} />
            {showPagination && (
                <Pagination
                    page={page}
                    totalPages={searchResult.total_pages}
                    onPageChange={setPage}
                />
            )}
        </>
    )
}

export default Home
