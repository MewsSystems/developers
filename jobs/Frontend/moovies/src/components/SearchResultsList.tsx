import styled from 'styled-components'
import Paginator from "./Paginator"
import Spinner from "./Spinner"
import ResultItem from "./ResultItem"
import { useAppSelector } from '../hooks/redux'
import { selectSearchResult } from '../redux/searchResultSlice'
import { selectQuery } from '../redux/querySlice'

const ResponsiveGrid = styled.div`
    display: grid;
    margin: 0 0.25rem;
    grid-template-rows: 1fr;
    grid-template-columns: 1fr;
    gap: 0.25rem 0.25rem;

    @media only screen and (min-width: 700px) {
        grid-template-columns: 1fr 1fr;
    }

    @media only screen and (min-width: 1100px) {
        grid-template-columns: 1fr 1fr 1fr;
    }   
`
export interface SearchResult {
    data: MovieObject[]
    loading: boolean
    error: boolean
    totalPages?: number
}
export interface MovieObject {
    id: string
    title: string
    tagline: string
    overview: string
    poster_path: string
    genres: GenreObject[]
    release_date: string
    credits: {
        cast: CastObject[]
        crew: object[]
    }
}
export interface CastObject {
    name: string
    character: string
    profile_path: string
}

interface GenreObject {
    id: string
    name: string
}

const SearchResultsList = (props: any) => {

    const query = useAppSelector(selectQuery)
    const searchResult = useAppSelector(selectSearchResult)

    if (query === "") {
        return (
            <div>Start by typing in a movie title</div>
        )
    }

    if (searchResult.loading) {
        return (
            <Spinner />
        )
    }

    if (searchResult.error) {
        return (
            <div>Oops. Something went wrong. Try again.</div>
        )
    }

    if (searchResult.data.length === 0) {
        return (
            <div>
                We couldn't find any movies.
            </div>
        )
    }

    const namesList = searchResult.data.map((movie: MovieObject) => (
        <ResultItem key={movie.id} data={movie} />
    ))

    return (
        <>
            <ResponsiveGrid>
                {searchResult.loading ? <Spinner /> : namesList}
            </ResponsiveGrid>
            <Paginator maxPages={searchResult.totalPages} />
        </>
    )
}

export default SearchResultsList
