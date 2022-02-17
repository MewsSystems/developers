import { useState, useEffect } from "react"

const useMovieDetail = (movieId: any) => {

    const [movieInfo, setMovieInfo] = useState()

    useEffect(() => {
        (async () => {
            try {
                const response = await fetch(`https://api.themoviedb.org/3/movie/${movieId}?api_key=03b8572954325680265531140190fd2a&language=en-US`)
                if (response.status === 200) {
                    const data = await response.json()
                    setMovieInfo(data)
                    console.log(data)
                }

                if (response.status === 401) {
                    throw new Error("401: invalid api key")
                }

                if (response.status === 404) {
                    throw new Error("404: movie id not found")
                }
            }
            catch (er) {
                console.error("failed to fetch movie detail (useMovieDetail): ", er)
            }
        })()
    }, [movieId])

    return movieInfo
}

export default useMovieDetail
