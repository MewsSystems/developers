import { Link } from "react-router-dom"
import { useState, useEffect } from "react"
import useSearch from "../hooks/useSearch"
import Paginator from "./Paginator"
import Spinner from "./Spinner"

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
}

const SearchResults = (props: any) => {

    const { query } = props
    const [searchResult, setSearchResult] = useState<SearchResult>({ data: [], error: false, loading: false })
    const [isLoading, setIsLoading] = useState(false)
    const [currPage, setCurrPage] = useState(1)

    let data = useSearch(query, currPage)

    useEffect(() => {
        setSearchResult(data)
        setIsLoading(data.loading)
    })

    if (query === "") {
        return (
            <div>Start by typing in a movie title</div>
        )
    }

    if (isLoading) {
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

    const namesList = searchResult.data.map((movie) => (
        <Link
            to={`/detail/${movie.id}`}
            key={movie.id}
        >
            <p>{movie.title}</p>
        </Link>
    ))

    return (
        <>
            <div>
                {namesList}
            </div>
            <Paginator
                query={query}
                currPage={currPage}
                setCurrPage={setCurrPage}
                maxPages={searchResult.totalPages}
            />
        </>
    )
}

export default SearchResults
