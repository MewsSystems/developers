import { useParams } from "react-router"
import { useEffect } from "react"
import { useState } from "react"
import { getFormattedMovieID } from "../API/getFormattedMovieID"
import { MovieDetailInfo } from "../types/movieDetail"
import { MovieDetailContent } from "./MovieDetailContent"
import { Header } from "../Header"
import { Footer } from "../Footer"

export const MovieDetailPage: React.FC = () => {

    const [movie, setMovie] = useState<MovieDetailInfo | null>(null);

    const { title } = useParams<{ title: string }>()

    const id = Number(title?.split("-")[0])

    useEffect(() => {
        getFormattedMovieID(id).then((data) => {
            setMovie(data)
        })
    },[])

    if (!movie) return <p>Loading...</p>;

    return (
        <>
            <Header/>
            <main>
                <MovieDetailContent movie={movie}/>
            </main>
            <Footer/>
        </>
    )
}