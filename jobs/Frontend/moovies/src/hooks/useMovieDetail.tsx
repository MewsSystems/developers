import { useState, useEffect } from "react"
import { SearchResult } from "../components/SearchResultsList"

const useMovieDetail = (movieId?: string) => {

    const [movieInfo, setMovieInfo] = useState<SearchResult>({ data: [], error: false, loading: true })

    useEffect(() => {

        const doApiRequest = async () => {
            try {
                const apiKey = '03b8572954325680265531140190fd2a'
                const response = await fetch(`https://api.themoviedb.org/3/movie/${movieId}?` +
                    new URLSearchParams({
                        api_key: apiKey,
                        language: "en-US",
                        append_to_response: "credits"
                    }))
                if (response.status === 200) {
                    const data = await response.json()
                    setMovieInfo({ data: [data], error: false, loading: false })
                    console.log(data)
                }

                if (response.status === 401) {
                    setMovieInfo({ data: [], error: true, loading: false })
                    throw new Error("401: invalid api key")
                }

                if (response.status === 404) {
                    setMovieInfo({ data: [], error: true, loading: false })
                    throw new Error("404: movie id not found")
                }
            }
            catch (er) {
                console.error("failed to fetch movie detail (useMovieDetail): ", er)
            }
        }

        setMovieInfo({ data: [], error: false, loading: true })
        setTimeout(() => doApiRequest(), 200)
        // doApiRequest()

    }, [movieId])

    return movieInfo
}

export default useMovieDetail
