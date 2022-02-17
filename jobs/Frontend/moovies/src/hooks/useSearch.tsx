import { useState, useEffect } from "react"
import { SearchResult } from "../components/SearchResults"

const useSearch = (query: string, pageNum: number = 1): SearchResult => {

    const [results, setResults] = useState<SearchResult>({ data: [], error: false, loading: true })

    useEffect(() => {
        // reset resultsList if there is no query
        if (query === "") {
            setResults({ data: [], error: false, loading: false })
            return
        }

        setResults({ data: [], error: false, loading: true })

        const doApiRequest = async () => {
            try {
                const apiKey = '03b8572954325680265531140190fd2a'
                const response = await fetch('https://api.themoviedb.org/3/search/movie?' + new URLSearchParams({
                    api_key: apiKey,
                    language: 'en-US',
                    query,
                    page: pageNum.toString(),
                    include_adult: 'false',
                }))

                if (response.status === 200) {
                    const data = await response.json()
                    setResults({ data: data.results, error: false, loading: false, totalPages: data.total_pages })
                }

                if (response.status === 422) {
                    setResults({ data: [], error: true, loading: false })
                    throw new Error("query not found")
                }

                if (response.status === 503) {
                    setResults({ data: [], error: true, loading: false })
                    throw new Error("service not available")
                }
            }
            catch (err) {
                console.error("failed to fetch (useSearch): ", err)
            }
        }

        setTimeout(() => doApiRequest(), 1000)
        // doApiRequest()

    }, [query, pageNum])

    return results
}

export default useSearch
