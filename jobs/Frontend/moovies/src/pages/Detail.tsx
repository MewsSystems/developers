import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import useMovieDetail from "../hooks/useMovieDetail"
import { MovieObject, SearchResult } from "../components/SearchResults"
import Spinner from "../components/Spinner"

type DetailParams = {
    movieId: string
}

const Detail = (props: any) => {

    let { movieId } = useParams<DetailParams>()

    const [movieData, setMovieData] = useState<MovieObject>()
    // const [isLoading, setIsLoading] = useState(true)
    const result = useMovieDetail(movieId)

    useEffect(() => {
        setMovieData(result.data[0])
        // setIsLoading(result.loading)
    })

    if (result.loading) {
        return (
            <Spinner />
        )
    }

    if (result.error || movieData === undefined) {
        return (
            <p>Oops. Couldnt find any movie with this ID.</p>
        )
    }

    return (
        <div>
            <p>movie detail: {movieId}</p>
            <p>{movieData.title}</p>
            <p>{movieData.tagline}</p>
            <p>{movieData.overview}</p>
        </div>

    )
}

export default Detail
