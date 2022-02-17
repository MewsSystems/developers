import { useState, useEffect } from "react"

const useSearch = (query: string, pageNum: number) => {

    const [results, setResults] = useState([])

    useEffect(() => {
        (async () => {
            try {
                const response = await fetch(`https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a&language=en-US&query=${query}&page=${pageNum}&include_adult=false`)
                console.log(response)

                if (response.status === 200) {
                    const data = await response.json()
                    setResults(data)
                }

                if (response.status === 422) {
                    setResults([])
                    throw new Error("query not found")
                }

                if (response.status === 503) {
                    setResults([])
                    throw new Error("service not available")
                }
            }
            catch (er) {
                console.error("failed to fetch (useSearch): ", er)
            }
        })()
    }, [query, pageNum])

    return results
}

export default useSearch
