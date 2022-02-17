import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import useMovieDetail from "../hooks/useMovieDetail"

const Detail = (props: any) => {

    interface MovieObject {
        title: string,
        tagline: string,
        overview: string,
    }

    const { movieId } = useParams()
    const [movieData, setMovieData] = useState<MovieObject>()
    const result = useMovieDetail(movieId)

    useEffect(() => {
        if (result === undefined) {
            return
        }
        setMovieData(result)
    })

    if (movieData === null || movieData === undefined) {
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
