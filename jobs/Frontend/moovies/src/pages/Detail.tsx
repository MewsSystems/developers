import styled from 'styled-components'
import { useEffect, useState } from "react"
import { useParams, Link } from "react-router-dom"
import useMovieDetail from "../hooks/useMovieDetail"
import { MovieObject } from "../components/SearchResultsList"
import Spinner from "../components/Spinner"

const H1 = styled.div`
    font-size: 2rem;
    line-height: 2;
`

const Img = styled.img`
    height: 500px;
    width: auto;
`

const P = styled.p`
    font-size: 1rem;
    line-height: 1.5;
`


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
            <>
                <Link to={"/"}>Back to search</Link>
                <p>Oops. Couldnt find any movie with this ID.</p>
            </>
        )
    }

    return (
        <>
            <Link to={"/"}>Back to search</Link>
            <div>
                <Img src={`https://image.tmdb.org/t/p/w500/${movieData.poster_path}`} />
                <H1>{movieData.title}</H1>
                <P>{movieData.overview}</P>
                <p>{movieData.tagline}</p>
            </div>
        </>

    )
}

export default Detail
