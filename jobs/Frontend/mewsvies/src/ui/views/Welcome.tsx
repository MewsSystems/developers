import { useState, useEffect } from 'react'
import { debounce } from '../utils/debounce'

import Search from '../components/Search.tsx'
import { getMovieSearch } from '../../domain/services/movie.service.ts'
import { MovieSearchResult } from '../../domain/interfaces/movie.interface.ts'
import MovieList from '../components/MovieList.tsx'

function Welcome() {
    const [searchResult, setSearchResult] = useState<MovieSearchResult>()
    const [query, setQuery] = useState('')
    const [page, setPage] = useState(1)

    useEffect(() => {
        // TODO refactor approach
        const searchMovie = async () => {
            if (!query) {
                return
            }

            try {
                const list = await getMovieSearch({ query: query, page })
                setSearchResult(list)
            } catch (error) {
                console.error(error)
            }
        }
        searchMovie()
    }, [query, page])

    return (
        <>
            <Search onSearch={debounce(setQuery, 500)} />
            <MovieList movies={searchResult?.results || []} />
        </>
    )
}

export default Welcome
